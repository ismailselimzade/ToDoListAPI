using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs;
using ToDoListAPI.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordService _passwordService;

        public UserController(AppDbContext appDbContext, IPasswordService passwordService)
        {
            this._db = appDbContext;
            this._passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                return BadRequest("Username required");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Password required");

            var user = new User { UserName = userDto.UserName, PasswordHash = _passwordService.HashPassword(userDto.Password) };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Ok(new GetUsersDto { Id = user.Id, UserName = user.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.Users
                .Select(u => new GetUsersDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _db.Users
                .Where(u => u.Id == userId)
                .Select(u => new GetUsersDto { UserName = u.UserName, Id = u.Id })
                .FirstOrDefaultAsync();

            return user == null ? NotFound(): Ok(user);

        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updateUserDto.UserName))
                return BadRequest("Username required");

            if (string.IsNullOrWhiteSpace(updateUserDto.OldPassword))
                return BadRequest("Old password required");

            if (string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
                return BadRequest("New password required");


            user.UserName = updateUserDto.UserName;
            if (_passwordService.VerifyPassword(updateUserDto.OldPassword, user.PasswordHash))
            {
                user.PasswordHash = _passwordService.HashPassword(updateUserDto.NewPassword);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                return Ok(new GetUsersDto { Id = user.Id, UserName = user.UserName });
            }
            else
            {
                return BadRequest("Password is not correct");
            }
            
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId, [FromQuery] string password)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (_passwordService.VerifyPassword(password, user.PasswordHash))
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest("Password is not correct");
        }
    }
}
