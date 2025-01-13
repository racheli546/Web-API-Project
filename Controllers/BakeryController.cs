using Microsoft.AspNetCore.Mvc;
using OurBakeryStore.Models;
using System.Collections.Generic;
using OurBakeryStore.Services;
using OurBakeryStore.Interfaces;

namespace OurBakeryStore.Controllers;

[ApiController]
[Route("[controller]")]
public class BakeryController : ControllerBase
{
    private readonly IBakeryService BakeryService;

    // קונסטרקטור רגיל להזרקת התלות
    public BakeryController(IBakeryService bakeryService)
    {
        BakeryService = bakeryService;
    }

    [HttpGet]
    public ActionResult<List<Bakery>> GetAll() =>
        BakeryService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Bakery> Get(int id)
    {
        var bakery = BakeryService.Get(id);
        if (bakery == null)
            return BadRequest("Invalid ID");
        return bakery;
    }


    [HttpPost]
    public IActionResult Create(Bakery newBakery)
    {
        BakeryService.Add(newBakery);
        Console.WriteLine($"POST: Added bakery with ID {newBakery.Id}");
        return CreatedAtAction(nameof(Get), new { id = newBakery.Id }, newBakery);
    }


    [HttpPut("{id}")]
    public IActionResult Update(int id, Bakery newBakery)
    {
        if (id != newBakery.Id) 
            return BadRequest();

        var existing = BakeryService.Get(id);
        if (existing == null) 
            return NotFound();

        BakeryService.Update(newBakery);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var bakery = BakeryService.Get(id);
        if (bakery == null)
            return NotFound();
        BakeryService.Delete(id);
        return Ok($"Bakery with ID {id} has been deleted.");
    }
}
