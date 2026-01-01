using System;

namespace webapp.Models;

public class Shop
{
    public int Id {get; set;}

    public string? Name {get; set;}

    public ICollection<Pizza>? Pizzas {get; set;}
}
