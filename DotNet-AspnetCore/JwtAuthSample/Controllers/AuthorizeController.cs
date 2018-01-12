using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace JwtAuthSample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthorizeController : Controller
    {
        private JwtSeetings _jwtSettings;

        public AuthorizeController(IOptions<JwtSeetings> _jwtSettingsAcceser)
        {
            _jwtSettings = _jwtSettingsAcceser.Value;
        }

        [HttpPost]
        public string Test()
        {
            return "1";
        }


        [HttpPost]
        public IActionResult Token(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!(viewModel.User == "Jesse" && viewModel.Password == "12345678"))
                {
                    return BadRequest();
                }

                var clamis = new Claim[] {
                    new Claim(ClaimTypes.Name, "Jesse"),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    clamis,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    credentials);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token)});
            }

            return BadRequest();
        }
    }
}
