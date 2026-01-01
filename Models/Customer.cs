using System;

namespace webapp.Models;

public class Customer
{
    public int Id {get; set;}

    public required string? Name {get; set;}

    public ICollection<Pizza>? Pizzas {get; set;} = new List<Pizza>(); 
}
