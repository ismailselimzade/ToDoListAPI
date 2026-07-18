using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext appDbContext, IPasswordService passwordService, IConfiguration configuration)
        {
            this._db = appDbContext;
            this._passwordService = passwordService;
            this._configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _db.Users
                .Where(u => u.UserName == loginDto.UserName)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                if (_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds
                        );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                    return BadRequest("username or password false");
            }
            else
                return BadRequest("username or password false");
        }
    }
}
