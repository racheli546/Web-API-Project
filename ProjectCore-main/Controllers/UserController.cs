using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Interfaces;
using ProjectCore.Services;

namespace ProjectCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            Console.WriteLine($" קיבלנו בקשת עדכון למשתמש ID: {id}");
            Console.WriteLine($" נתונים מהקליינט - ID: {user.Id}, שם משתמש: {user.Username}, תפקיד: {user.Role}");

            if (id != user.Id)
            {
                Console.WriteLine("❌ שגיאה: ה-ID שנשלח לא תואם לנתוני המשתמש!");
                return BadRequest(new { error = "User ID mismatch." });
            }

            var existingUser = _userService.Get(id);
            if (existingUser == null)
            {
                Console.WriteLine("❌ שגיאה: המשתמש לא נמצא במערכת!");
                return NotFound(new { error = "User not found." });
            }

            // ✅ עדכון פרטי המשתמש
            existingUser.Username = user.Username;
            existingUser.Role = user.Role;
            if (user.Password != null)
            {
                existingUser.Password = user.Password;
            }
            _userService.Update(existingUser);

            Console.WriteLine("✅ המשתמש עודכן בהצלחה!");
            return Ok(new { message = "User updated successfully.", user = existingUser });
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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] User user)
        {
            _userService.Add(user);
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }

    }
}