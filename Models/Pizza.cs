using System;
namespace webapp.Models;

public class Pizza
{
    public int Id {get; set;}

    public required string? Name {get; set;}

    public bool IsExtraCheese {get; set;}

    public bool IsGlutenFree {get; set;}
    
    public Shop? Shop {get;set;}

    public ICollection<Customer> Customers {get; set;} = new List<Customer>(); 
}
