// using Microsoft.AspNetCore.Mvc;
using OurBakeryStore.Models;
using System.Collections.Generic;
using System.Linq;
using OurBakeryStore.Services;
using OurBakeryStore.Interfaces;

namespace OurBakeryStore.Controllers;

[ApiController]
[Route("[controller]")]
public class BakeryController : ControllerBase
{
    private IBakeryService BakeryService;
    static BakeryController(IBakeryService BakeryService)
    {
        this.BakeryController = BakeryService;
    } 

    [HttpGet]
    public ActionResult<List<Bakery>> GetAll() =>
            BakeryService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Bakery> Get(int id)
    {
        var Bakery = BakeryService.Get(id);
        if (Bakery == null)
            return BadRequest("invalid id");
        return Bakery;
    }


    [HttpPost]
    public IActionResult create (Bakery newBakery)
    {        
        BakeryService.Add(newBakery);
        return CreatedAtAction(nameof(Insert), new { id = newBakery.Id }, newBakery);
    }  

    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Bakery newBakery)
    { 
       if(id != newBakery.Id) return BadRequest();
       var existing = bakeryService.Get(id);
       if(existing == null) return NotFound();
       bakeryService.Update(newBakery)
       return NoContent();
    } 

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var bakery = bakeryService.Get(id);
        if (bakery == null)
            return NotFound();
        bakeryService.Delete(id);
        return Ok($"Bakery with ID {id} has been deleted."); 
    }
}
