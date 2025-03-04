// using ProjectCore;
// using System.Security.Claims;
// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;


// namespace ProjectCore.Services
// {
//     public class AuthService : IAuthService
//     {
//         private readonly IConfiguration _config;
//         public AuthService(IConfiguration config)
//         {
//             _config = config;
//         }
//         public string GenerateJwtToken(User user)
//         {
//             //יצירת claims בשביל להכניס אותם לטוקן 
//             var claims = new[]
//             {
//                 new Claim(ClaimTypes.Name, user.Username),
//                 new Claim(ClaimTypes.Role, user.Role)
//             };
//             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RacheliSegal"));
//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//             private static string issuer = "https://fbi-demo.com";
//             public static SecurityToken GetToken(List<Claim> claims) =>
//             new JwtSecurityToken(
//                 issuer,
//                 issuer,
//                 claims,
//                 expires: DateTime.UtcNow.AddHours(1),
//                 signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
//             );
//             public static TokenValidationParameters GetTokenValidationParameters() =>
//             new TokenValidationParameters
//             {
//                 ValidIssuer = issuer,
//                 ValidAudience = issuer,
//                 // IssuerSigningKey = key,
//                 ClockSkew = TimeSpan.Zero // remove delay of token when expire
//             };

//             public static string WriteToken(SecurityToken token) =>
//                 new JwtSecurityTokenHandler().WriteToken(token);

//         }
//     }
// }


using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ProjectCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
         public AuthService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); 
            _issuer = _config["Jwt:Issuer"];
        }

        public string GenerateJwtToken(User user)
        {
             var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

           var token = new JwtSecurityToken(
                _issuer,
                _issuer,
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidAudience = _issuer,
                IssuerSigningKey = _key,
                ClockSkew = TimeSpan.Zero // הסרת השהייה של תוקף הטוקן
            };
    }
}
