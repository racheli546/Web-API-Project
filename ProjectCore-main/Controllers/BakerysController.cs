
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Interfaces;
namespace ProjectCore.Controllers;
[ApiController]
[Route("[controller]")]
public class BakerysController : ControllerBase
{
    private IBakeryService BakeryService;
    public BakerysController(IBakeryService BakeryService)
    {
        this.BakeryService = BakeryService;
    }


    [HttpGet]
    public IEnumerable<Bakery> GetAll()
    {
        return BakeryService.GetAll();
    }
    [HttpGet("{id}")]
    public ActionResult<Bakery> Get(int id)
    {
        var bakery = BakeryService.Get(id);

        if (bakery == null)
            return NotFound();

        return bakery;
    }
    [HttpPost]
    public ActionResult Insert(Bakery nb)
    {
        BakeryService.Add(nb);
        return CreatedAtAction(nameof(Insert), new { id = nb.Id }, nb);
    }
    [HttpPut("{id}")]
    public ActionResult Update(int id, Bakery nb)
    {
        if (id != nb.Id)
            return BadRequest();
        var existingBakery = BakeryService.Get(id);
        if (existingBakery is null)
            return NotFound();
        BakeryService.Update(nb);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
       var bakery = BakeryService.Get(id);
            if (bakery is null)
                return  NotFound();

            BakeryService.Delete(id);

            return Content(BakeryService.Count.ToString());
    }
}