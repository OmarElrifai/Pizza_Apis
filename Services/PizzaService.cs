
using System;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using webapp.Models;
namespace webapp.Services;


public  class PizzaService
{

    
    private MySqlConnection InitiateConnection()
    {
        var connectionString = "server=localhost;port=10002;database=pizza_db;user=admin;password=pizza123;";
        MySqlConnection conn = new MySqlConnection(connectionString);
        return conn;
    }

    public  async Task<List<Pizza>> GetPizzas()
    {
        var conn = InitiateConnection();
        try
        {
            Console.WriteLine("Connecting to MySql Db");
            conn.Open();
            var sql = "SELECT o.Id, o.Name, o.IsGlutenFree, o.IsExtraCheese FROM Pizzas o";
            IEnumerable<Pizza> pizzas =  await conn.QueryAsync<Pizza>(sql);

            foreach (Pizza pizza in pizzas)
            {
                Console.WriteLine(pizza+" -- "+pizza.Name);
            }
            conn.Close();

            return pizzas.ToList();

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new List<Pizza>();
        }
        
    }



    public async Task<List<Pizza>> GetPizzasWithShops()
    {
        var conn = InitiateConnection();
        try
        {
            Console.WriteLine("Connecting to MySql Db");
            conn.Open();
            var sql = @"SELECT o.Id, o.Name, o.IsGlutenFree, o.IsExtraCheese, o.Shop, s.Id, s.Name FROM Pizzas o 
            LEFT JOIN Shops s ON o.Shop = s.Id";
            IEnumerable<Pizza> pizzas =  await conn.QueryAsync<Pizza,Shop,Pizza>(sql,(pizza, shop) =>
            {
                pizza.Shop = shop.Id > 0? shop: null ;
                return pizza;
            },splitOn:"Shop");

            foreach (Pizza pizza in pizzas)
            {
                Console.WriteLine(pizza+" -- "+pizza.Name);
            }
            conn.Close();

            return pizzas.ToList();

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new List<Pizza>();
        }
        
    }

    // public  async Task<Pizza> GetPizzasWithShops()
    // {
    //     var pizzas = await db.Pizzas
    //         .OrderBy(p => p.Id)
    //         .FirstAsync();
    //     return pizzas;
       
        
    // }

    public  async Task<List<Pizza>> GetPizzasWithCustomers()
    {
        var conn = InitiateConnection();
        try
        {
            Console.WriteLine("Connecting to MySql Db");
            conn.Open();
            var sql = @"SELECT p.Id, p.Name, p.IsGlutenFree, p.IsExtraCheese, c.Id, c.Name FROM Pizzas p 
            LEFT JOIN PizzaCustomers pc ON p.Id = pc.Pizza
            LEFT JOIN Customers c ON  pc.Customer = c.Id";

            List<Pizza> result = new List<Pizza>();            
            await conn.QueryAsync<Pizza,Customer,Pizza>(sql,(pizza, customer) =>
            {
                Pizza singlePizza = result.SingleOrDefault(p => p.Id == pizza.Id);
                if (customer is null)
                {
                    pizza.Customers = [];
                    result.Add(pizza);
                }
                else
                {
                    if (singlePizza is null)
                    {
                        pizza.Customers.Add(customer);
                        result.Add(pizza);
                    }
                    else
                    {
                        singlePizza.Customers.Add(customer);
                    }                    
                }
                

                return pizza;
            });

            foreach (Pizza pizza in result)
            {
                Console.WriteLine(pizza+" -- "+pizza.Name);
            }
            conn.Close();

            return result;

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new List<Pizza>();
        }
        
    }

     

    public async Task<Pizza> GetPizza(int id)
    {
        var conn = InitiateConnection();
        try
        {
          var sql = "SELECT p.Id, p.Name, p.IsExtraCheese, p.IsGlutenFree FROM Pizzas p WHERE Id = @id";
          var pizza = await conn.QuerySingleAsync<Pizza>(sql,new {id=id}); 
          return pizza; 
        }
        catch(Exception e)
        {
            throw e;
        }
         
    } 

    public async Task<Shop> GetShop(int? id)
    {
        var conn = InitiateConnection();
        try
        {
          var sql = "SELECT s.Id, s.Name FROM Shops s WHERE Id = @id";
          var shop = await conn.QuerySingleOrDefaultAsync<Shop>(sql,new {id=id}); 
          return shop; 
        }
        catch (InvalidOperationException ex)
        {
            // Handle specific SQL exceptions (e.g., log the error, notify the user)
            Console.WriteLine($"SQL Error: {ex.Message}");
            // You might return an empty list or a default value
            return null;
        }
        catch (Exception ex)
        {
            // Handle other general exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
         
    }

    public async Task<Shop> GetShopByName(string? name)
    {
        try
        {
            var conn = InitiateConnection();
            var sql = "SELECT s.Id, s.Name FROM Shops s WHERE Name = @name";
            var shop = await conn.QuerySingleOrDefaultAsync<Shop>(sql,new { name }); 
            return shop;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return null;
        }
        
         
    } 

    public async Task<Customer> GetCustomer(int? id)
    {
        var conn = InitiateConnection();
        try
        {
          var sql = "SELECT c.Id, c.Name FROM Customers c WHERE Id = @id";
          var pizza = await conn.QuerySingleOrDefaultAsync<Customer>(sql,new {id=id}); 
          return pizza; 
        }
        catch(Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return null;
        }
         
    } 

    public async Task<Customer> GetCustomerByName(string? name)
    {
        var conn = InitiateConnection();
        try
        {
          var sql = "SELECT c.Id, c.Name FROM Customers c WHERE c.Name = @name";
          var pizza = await conn.QuerySingleOrDefaultAsync<Customer>(sql,new {name}); 
          return pizza; 
        }
        catch(Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            return null;
        }
         
    } 


    public  async Task<Pizza> Add(Pizza pizza)
    {
        return await CreateUpdate(false,pizza);
         
    }

    public  async Task<Pizza> Update(Pizza pizza)
    {
        return await CreateUpdate(true,pizza);
        
    }

    public async Task<Pizza> CreateUpdate(bool isUpdate, Pizza pizza)
    {
        var shopId = pizza.Shop?.Id;
   
        if (isUpdate)
        {
            return await UpdatePizza(pizza);
        }
        else
        {
            return await InsertPizza(pizza);
        }


    }

    public async Task<Pizza>InsertPizza(Pizza pizza)
    {
        var conn = InitiateConnection();
        var sql="";
        var shop = pizza.Shop;
        var shopId = 0;
        if (shop is null)
        {
            sql = "INSERT INTO Pizzas (Name, IsExtraCheese, IsGlutenFree) VALUES (@name, @isExtraCheese, @isGlutenFree)";    
        }
        else
        {
            shopId = await InsertShop(shop);
            sql = $"INSERT INTO Pizzas (Name, IsExtraCheese, IsGlutenFree,Shop) VALUES (@name, @isExtraCheese, @isGlutenFree,{shopId})";

        }

        try
        {   
            var rowAffected = await conn.ExecuteAsync(sql,pizza);
            return pizza;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public async Task<Pizza>UpdatePizza(Pizza pizza)
    {
        var conn = InitiateConnection();
        var sql="";
        var shop = pizza.Shop;
        var shopId = 0;
        if (shop is null)
        {
            sql =  "UPDATE Pizzas SET Name = @name, IsGlutenFree = @isGlutenFree, IsExtraCheese = @isExtraCheese WHERE Id = @id";
        }
        else
        {
            
            shopId = await InsertShop(shop);
            sql = $"UPDATE Pizzas SET Name = @name, IsGlutenFree = @isGlutenFree, IsExtraCheese = @isExtraCheese, Shop = {shopId} WHERE Id = @id";

        }

        try
        {   
            var rowAffected = await conn.ExecuteAsync(sql,pizza);
            return pizza;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public async Task<int> InsertShop(Shop shop)
    {
        var conn = InitiateConnection();
        var retievedShop = await GetShopByName(shop.Name);
        if (retievedShop is null)
        {
            var shopInsertSql = @"INSERT INTO Shops (Name) VALUES (@name); SELECT LAST_INSERT_ID() AS NewShopId;";
            return await conn.QuerySingleAsync<int>(shopInsertSql,shop);
        }
        else
        {
            return retievedShop.Id;
        }
    }

    public async Task<Pizza> AddCustomersToPizza(Pizza pizza)
    {
        var conn = InitiateConnection();
        foreach (Customer customer in pizza.Customers)
        {
            var customerId = await InsertCustomer(customer);
            var sqlCommand = $"INSERT INTO PizzaCustomers (Pizza, Customer) VALUES ({pizza.Id},{customerId})";
            try
            {
                await conn.QueryAsync(sqlCommand);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        return pizza;
    }

    public async Task<int> InsertCustomer(Customer customer)
    {
        var conn = InitiateConnection();
        var retievedCustomer = await GetCustomerByName(customer.Name);
        if (retievedCustomer is null)
        {
            var shopInsertSql = @"INSERT INTO Customers (Name) VALUES (@name); SELECT LAST_INSERT_ID() AS NewCustomerId;";
            return await conn.QuerySingleAsync<int>(shopInsertSql,customer);
        }
        else
        {
            return retievedCustomer.Id;
        }
    }

    public async Task<int> Remove(int pizzaInt)
    {
        // if (GetPizza(id) is null)
        // {
        //     return "No record found for this pizza";
        // }
        // else
        // {       
        //     Pizzas.RemoveAll(pizza => pizza.Id == id);
        //     return "Order removed successfully";
        // }
        var connectionString = "server=localhost;port=10002;database=pizza_db;user=admin;password=pizza123;";
        var conn = new MySqlConnection(connectionString);
        try
        {
            var deleteAllChildren = "DELETE FROM PizzaCustomers WHERE Pizza = @id";
            await conn.ExecuteAsync(deleteAllChildren, new {id = pizzaInt});
            var sql = "DELETE FROM Pizzas WHERE Id = @id";
            return await conn.ExecuteAsync(sql, new {id = pizzaInt});

        }
        catch (Exception e)
        {
            throw e;
        }     
        
    }
    

    
}
