using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs;
using ToDoListAPI.Models;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                return BadRequest("Username required");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Password required");

            var user = new User { UserName = userDto.UserName, PasswordHash = _passwordService.HashPassword(userDto.Password) };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return Ok(new GetUserDto { Id = user.Id, UserName = user.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _db.Users
                .Where(u => u.Id == userId)
                .Select(u => new GetUserDto { UserName = u.UserName, Id = u.Id })
                .FirstOrDefaultAsync();

            return user == null ? NotFound(): Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updateUserDto.OldPassword))
                return BadRequest("Old password required");

            if (string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
                return BadRequest("New password required");

            

            if (_passwordService.VerifyPassword(updateUserDto.OldPassword, user.PasswordHash))
            {
                user.UserName = updateUserDto.UserName;
                user.PasswordHash = _passwordService.HashPassword(updateUserDto.NewPassword);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                return Ok(new GetUserDto { Id = user.Id, UserName = user.UserName });
            }
            else
            {
                return BadRequest("Password is not correct");
            }
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserDto deleteUserDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _db.Users.FindAsync(userId);

            if (user == null) return NotFound();

            if (_passwordService.VerifyPassword(deleteUserDto.Password, user.PasswordHash))
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
