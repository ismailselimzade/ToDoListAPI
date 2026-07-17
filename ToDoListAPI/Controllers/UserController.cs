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
            var user = new User { UserName = userDto.UserName, PasswordHash = _passwordService.HashPassword(userDto.PasswordHash) };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUsersDto>>> GetUsers()
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
    }
}
