using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Interfaces;
using ProjectCore.Services;

namespace ProjectCore.Controllers{
[ApiController]
[Route("[controller]")]
// [Route("api/[controller]")]
// [Route("api/Users")]
// [Authorize(Policy = "User")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly int userId = 1;

    public UserController(IUserService UserService, IHttpContextAccessor httpContextAccessor)
    {
        this._userService = UserService;
       var userIdClaim = httpContextAccessor?.HttpContext?.User.FindFirst("Id")?.Value;
        if (!int.TryParse(userIdClaim, out this.userId))
        {
            Console.WriteLine("⚠️ שגיאה: לא נמצא Id למשתמש מחובר");
            this.userId = 1; // נשתמש בערך ברירת מחדל
        }
    }


    // [HttpGet()]
    // [Authorize(Policy = "Admin")]
    // public ActionResult<List<User>> GetAllUsers() => UserService.GetAll();



    // [HttpGet("currentUser")]
    // public ActionResult<User> getUser()
    // {
    //     Console.WriteLine(" נכנסנו לפונקציה getUser");
    //     return _userService.Get(this.userId);
    // }
    // [HttpGet]
    // public ActionResult<User> getUser()
    // {
    //     try
    //     {
    //         Console.WriteLine(" נכנסנו לפונקציה getUser");
    //         var user = _userService.Get(this.userId);

    //         if (user == null)
    //         {
    //             Console.WriteLine(" המשתמש לא נמצא");
    //             return NotFound("User not found");
    //         }

    //         return user;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"💥 שגיאה חמורה: {ex.Message}");
    //         return StatusCode(500, "An unexpected error occurred.");
    //     }
    // }


    // [HttpPut]
    // // [Route("currentUser")]
    // public ActionResult updateCurrentUser(User user)
    // {
    //     if (this.userId != user.Id)
    //         return BadRequest();
    //     _userService.Update(user);
    //     return NoContent();
    // }
[HttpPut]
public ActionResult updateCurrentUser([FromBody] User user)
{
    Console.WriteLine($"🔍 נתונים שהתקבלו: user.Id = {user.Id}, this.userId = {this.userId}");

    if (user == null)
    {
        Console.WriteLine("❌ המשתמש הוא null");
        return BadRequest("User data is missing.");
    }

    if (this.userId != user.Id)
    {
        Console.WriteLine($"❌ ID לא תואם: this.userId = {this.userId}, user.Id = {user.Id}");
        return BadRequest("User ID mismatch.");
    }

    _userService.Update(user);
    Console.WriteLine("✅ המשתמש עודכן בהצלחה");
    return NoContent();
}



   [HttpGet]
public ActionResult<List<User>> GetAll()
{
    Console.WriteLine(" נכנסנו לפונקציה GetAll");
    return _userService.GetAll();
}

    // [HttpGet("{id}")]
    // // [Authorize(Policy = "Admin")]
    // public ActionResult<User> GetById(int id) => _userService.Get(id);

    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        Console.WriteLine($" GetById נקרא עם id = {id}");
        var user = _userService.Get(id);
        
        if (user == null)
        {
            Console.WriteLine(" המשתמש לא נמצא");
            return NotFound();
        }
        
        return Ok(user);
    }

    // [HttpDelete("{id}")]
    // // [Authorize(Policy = "Admin")]
    // public IActionResult Delete(int id)
    // {
    //     var existingTask = _userService.Get(id);
    //     if (existingTask is null)
    //         return NotFound();
    //     _userService.Delete(id);
    //     return NoContent();
    // }

    [HttpDelete("{id}")]
    // [Authorize(Policy = "Admin")]
    public IActionResult Delete(int id)
    {
        Console.WriteLine($"🔍 מנסים למחוק משתמש עם id = {id}");

        var existingUser = _userService.Get(id);
        if (existingUser == null)
        {
            Console.WriteLine(" המשתמש לא נמצא");
            return NotFound("User not found.");
        }

        _userService.Delete(id);
        Console.WriteLine(" המשתמש נמחק בהצלחה");
        return NoContent();
    }

    [HttpPost]
    // [Authorize(Policy = "Admin")]
    public IActionResult Create(User user)
    {
        _userService.Add(user);
        return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
    }
    // [HttpGet("{id}")]
    // [Authorize(Policy = "Admin")]
    // public ActionResult<User> GetAll(int id) => _userService.GetAll(id);

    // [HttpGet("{id}")]
    // [Authorize]
    // public ActionResult<User> GetUser(int id)
    // {
    //     var user = _userService.Get(id);
    //     return Ok(user);
    // }

//   [HttpGet]
//         [Route("currentUser")]
//         public ActionResult<User> getUser() =>
//                     UserService.Get(this.userId);

//     [HttpPost]
//         [Authorize(Policy = "Admin")]
//     public IActionResult create( User user)
//     {
//         UserService.Add(user);
//         return CreatedAtAction(nameof(Created), new { id = user.Id }, user);
//     }

//     // [HttpPut("{id}")]
//     // [Authorize]
//     // public ActionResult UpdateUser(int id, [FromBody] User user)
//     // {
//     //     if (id != user.Id)
//     //         return BadRequest();

//     //     _userService.Update(user);
//     //     return NoContent();
//     // }

//     [HttpPut]
//     [Route("currentUser")]
//     [Authorize]
//     public ActionResult UpdateUser( User user)
//     {
//        if (this.userId != user.Id)
//             return BadRequest();
//         UserService.Update(user);
//         return NoContent();
//     }

//     [HttpDelete("{id}")]
//     [Authorize(Policy = "Admin")]
//     public IActionResult DeleteUser(int id)
//     {
//         var existingTask = UserService.Get(id);
//             if (existingTask is null)
//                 return NotFound();

//             UserService.Delete(id);
//             return NoContent();
    }
}