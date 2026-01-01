using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapp.Models;
using webapp.Services;

namespace webapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private PizzaService PizzaService;
        public PizzaController(PizzaService pizzaService)
        {
            PizzaService = pizzaService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Pizza>>> GetAllPizzas() => await PizzaService.GetPizzas();


        [HttpGet("getWithShops")]
        public async Task<ActionResult<List<Pizza>>> GetPizzasWithShops() => await PizzaService.GetPizzasWithShops();

        [HttpGet("getWithCustomers")]
        public async Task<ActionResult<List<Pizza>>> GetPizzasWithCustomers() => await PizzaService.GetPizzasWithCustomers();

        [HttpGet("get/{id}")]
        public Task<Pizza> GetPizza(int id)  {

            return PizzaService.GetPizza(id);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddPizza(Pizza pizza) {

            await PizzaService.Add(pizza);
            var pizzas = await PizzaService.GetPizzas();
            var createdPizza = pizzas[pizzas.Count - 1];
            var createdPizzaId = createdPizza.Id;
            return CreatedAtAction(nameof(GetPizza),new {id = createdPizzaId},createdPizza);

        }

        [HttpPut]
        public async Task<ActionResult> UpdatePizza(Pizza pizza)  {
            return Ok(await PizzaService.Update(pizza));
        }

        [HttpPost("addCustomersToPizza")]
        public async Task<ActionResult> AddCustomersToPizza(Pizza pizza)
        {
            return Ok(await PizzaService.AddCustomersToPizza(pizza));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePizza(int id)
        {
            await PizzaService.Remove(id);
            return Ok("OK");
        }
        

    }
}
