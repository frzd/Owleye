using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Owleye.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserModel login)
        {
           

            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateToken(user);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
             };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(5),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel user)
        {

            if (user.Username == "owleye" && user.Password == "123")
            {
                user = new UserModel { Username = "owl eye user", EmailAddress = "xx@yy.com" };
            }

            return null;
        }
    }
}
