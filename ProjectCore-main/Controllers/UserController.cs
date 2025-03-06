using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Interfaces;
using ProjectCore.Services;

namespace ProjectCore.Controllers
{
[ApiController]
[Route("[controller]")]
// [Route("api/[controller]")]
// [Route("api/Users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IBakeryService _bakeryService;
    private readonly int userId = 1;

    public UserController(IUserService UserService, IBakeryService BakeryService, IHttpContextAccessor httpContextAccessor)
    {
        this._userService = UserService;
         this._bakeryService = BakeryService;
       var userIdClaim = httpContextAccessor?.HttpContext?.User.FindFirst("Id")?.Value;
        if (!int.TryParse(userIdClaim, out this.userId))
        {
            Console.WriteLine(" שגיאה: לא נמצא Id למשתמש מחובר");
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
    //         Console.WriteLine($" שגיאה חמורה: {ex.Message}");
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
    Console.WriteLine($" נתונים שהתקבלו: user.Id = {user.Id}, this.userId = {this.userId}");

    if (user == null)
    {
        Console.WriteLine(" המשתמש הוא null");
        return BadRequest("User data is missing.");
    }

    if (this.userId != user.Id)
    {
        Console.WriteLine($" ID לא תואם: this.userId = {this.userId}, user.Id = {user.Id}");
        return BadRequest("User ID mismatch.");
    }

    _userService.Update(user);
    Console.WriteLine(" המשתמש עודכן בהצלחה");
    return NoContent();
}


   // ✅ משתמש רגיל יכול לעדכן את עצמו בלבד, Admin יכול לעדכן את כולם
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User user)
        {
            var loggedInUserId = int.Parse(User.FindFirst("Id")?.Value);

            if (User.IsInRole("Admin") || loggedInUserId == id)
            {
                _userService.Update(user);
                return NoContent();
            }

            return Forbid(); // ❌ לא מאפשר עריכת משתמשים אחרים
        }

    // ✅ רק משתמשים מחוברים יכולים לראות את הפרופיל שלהם
        [HttpGet("currentUser")]
        public ActionResult<User> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            return _userService.Get(userId);
        }

        // ✅ רק Admin יכול לראות את כל המשתמשים
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
        Console.WriteLine(" נכנסנו לפונקציה GetAll");
            return _userService.GetAll();
        }
    // [HttpGet("{id}")]
    // // [Authorize(Policy = "Admin")]
    // public ActionResult<User> GetById(int id) => _userService.Get(id);
  // ✅ רק Admin יכול לראות משתמש לפי ID
      
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<User> Get(int id)
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
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        Console.WriteLine($" מנסים למחוק משתמש עם id = {id}");

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
    [HttpPost("buy/{id}")]
[Authorize]
public ActionResult BuyBakery(int id)
{
    var userId = int.Parse(User.FindFirst("Id")?.Value);
    var bakery = _bakeryService.Get(id);

    if (bakery == null) return NotFound("Bakery not found.");

    _userService.BuyBakery(userId, bakery);

    return Ok($"✅ המאפה {bakery.Name} נרכש בהצלחה!");
}

[HttpDelete("remove/{id}")]
[Authorize]
public ActionResult RemoveBakery(int id)
{
    var userId = int.Parse(User.FindFirst("Id")?.Value);

    _userService.RemoveBakery(userId, id);

    return Ok("✅ המאפה הוסר בהצלחה!");
}

[HttpGet("purchases")]
[Authorize]
public ActionResult<List<Bakery>> GetUserPurchases()
{
    var userId = int.Parse(User.FindFirst("Id")?.Value);
    var user = _userService.Get(userId);

    if (user == null) return NotFound("User not found.");

    return Ok(user.PurchasedBakeries);
}

 
    // [HttpPost]
    // // [Authorize(Policy = "Admin")]
    // public IActionResult Create(User user)
    // {
    //     _userService.Add(user);
    //     return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
    // }
    // ✅ רק Admin יכול להוסיף משתמשים חדשים
// [HttpPost]
// [Authorize(Roles = "Admin")]
// public IActionResult CreateUser([FromBody] User user)
// {
//     _userService.Add(user);
//     return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
// }
[HttpPost]
[Authorize(Roles = "Admin")]
public IActionResult CreateUser([FromBody] User user)
{
    _userService.Add(user);
    return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
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