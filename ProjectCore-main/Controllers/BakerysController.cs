
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProjectCore.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Policy = "User")]

public class BakerysController : ControllerBase
{
    private IBakeryService BakeryService;
    public BakerysController(IBakeryService BakeryService)
    {
        this.BakeryService = BakeryService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        Console.WriteLine("📌 בקשת GET /Bakerys התקבלה");
        try
        {
            var bakeries = BakeryService.GetAll();
            Console.WriteLine("✅ BakeryService.GetAll() בוצע בהצלחה");

            if (bakeries == null)
            {
                Console.WriteLine("❌ BakeryService.GetAll() החזיר NULL!");
                return StatusCode(500, "Database error: No bakeries found.");
            }

            Console.WriteLine($"✅ נמצאו {bakeries.Count()} מאפים.");
            return Ok(bakeries);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה חמורה ב-GetAll: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"🔍 פרטי שגיאה: {ex.StackTrace}");
            return StatusCode(500, new { error = $"Internal Server Error: {ex.Message}" });
        }
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
    public ActionResult Update(int id, string name, int userId)
    {
        if (id != BakeryService.Get(id).Id)
        {
            return BadRequest(new { message = "ID לא תואם" });
        }

        var existingBakery = BakeryService.Get(id);
        if (existingBakery is null)
        {
            return NotFound(new { message = "מאפה לא נמצא" });
        }

        Bakery nb = new Bakery();
        nb.Id = id;
        nb.Name = name;
        nb.UserId = userId;

        BakeryService.Update(nb);
        return Ok(nb); // החזרת אובייקט JSON תקין
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var bakery = BakeryService.Get(id);
        if (bakery is null)
            return NotFound();

        BakeryService.Delete(id);

        return Content(BakeryService.Count.ToString());
    }
}