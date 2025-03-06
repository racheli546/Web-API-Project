using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCore;
using ProjectCore.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IBakeryService _bakeryService;

    public ItemController(IBakeryService bakeryService)
    {
        _bakeryService = bakeryService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<Bakery>> GetAllItems()
    {
        return Ok(_bakeryService.GetAll());
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<Bakery> GetItem(int id)
    {
        var item = _bakeryService.Get(id);
        return Ok(item);
    }

    [HttpPost]
    [Authorize]
    public ActionResult AddItem([FromBody] Bakery bakery)
    {
        _bakeryService.Add(bakery);
        return CreatedAtAction(nameof(GetItem), new { id = bakery.Id }, bakery);
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult UpdateItem(int id, [FromBody] Bakery bakery)
    {
        if (id != bakery.Id)
            return BadRequest();

        _bakeryService.Update(bakery);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult DeleteItem(int id)
    {
        _bakeryService.Delete(id);
        return NoContent();
    }
}
