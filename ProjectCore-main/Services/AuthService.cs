


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
