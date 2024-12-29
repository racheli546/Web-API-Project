using Microsoft.AspNetCore.Mvc;
using OurBakeryStore.Models;

namespace OurBakeryStore.Controllers;

[ApiController]
[Route("[controller]")]
public class BakeryController : ControllerBase
{
    private static List<Bakery> list;
    static BakeryController()
    {
        list = new List<Bakery> 
        {
            new Bakery { Id = 4, Name = "Chocolate Croissant", IsItWithChocolate = true },
            new Bakery { Id = 5, Name = "Chocolate Donut", IsItWithChocolate = true },
            new Bakery { Id = 6, Name = "Vanilla Muffin", IsItWithChocolate = false },
            new Bakery { Id = 7, Name = "Cinnamon Yeast Cake", IsItWithChocolate = false },
            new Bakery { Id = 8, Name = "Butter Cookies", IsItWithChocolate = false },
        };
    } 

    [HttpGet]
    public IEnumerable<Bakery> Get()
    {
        return list;
    }
    [HttpGet("{id}")]
    public ActionResult<Bakery> Get(int id)
    {
        var Bakery = list.FirstOrDefault(p => p.Id == id);
        if (Bakery == null)
            return BadRequest("invalid id");
        return Bakery;
    }

    [HttpPost]
    public ActionResult Insert(Bakery newBakery)
    {        
        var maxId = list.Max(p => p.Id);
        newBakery.Id = maxId + 1;
        list.Add(newBakery);

        return CreatedAtAction(nameof(Insert), new { id = newBakery.Id }, newBakery);
    }  

    
    [HttpPut("{id}")]
    public ActionResult Update(int id, Bakery newBakery)
    { 
        var oldBakery = list.FirstOrDefault(p => p.Id == id);
        if (oldBakery == null) 
            return BadRequest("invalid id");
        if (newBakery.Id != oldBakery.Id)
            return BadRequest("id mismatch");

        oldBakery.Name = newBakery.Name;
        oldBakery.IsItWithChocolate = newBakery.IsItWithChocolate;
        return NoContent();
    } 

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var bakery = list.FirstOrDefault(p => p.Id == id);
        if (bakery == null)
            return BadRequest("invalid id");
        list.Remove(bakery); 
        return Ok($"Bakery with ID {id} has been deleted."); 
    }
}
