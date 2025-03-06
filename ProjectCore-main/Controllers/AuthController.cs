using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectCore.Interfaces;
using ProjectCore.Models;

namespace ProjectCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
             _config = config;
        }

        // [HttpPost("login")]
        // public IActionResult Login([FromBody] UserLoginDto userDto)
        // {
        //     var user = _userService.Authenticate(userDto.Username, userDto.Password);
        //     if (user == null)
        //         return Unauthorized("שם משתמש או סיסמה לא נכונים");

        //     var token = GenerateJwtToken(user);
        //     return Ok(new { Token = token });
        // }
        [HttpPost("login")]
public IActionResult Login([FromBody] UserLoginDto loginDto)
{
    try
    {
        Console.WriteLine("🔍 התחלת תהליך התחברות");

        var user = _userService.Authenticate(loginDto.Username, loginDto.Password);
        if (user == null)
        {
            Console.WriteLine("❌ התחברות נכשלה – שם משתמש או סיסמה שגויים");
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        Console.WriteLine($"✅ התחברות הצליחה! טוקן נוצר: {token}");

        return Ok(new { token });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 שגיאה חמורה בעת התחברות: {ex.Message}");
        return StatusCode(500, "An unexpected error occurred.");
    }
}


        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
