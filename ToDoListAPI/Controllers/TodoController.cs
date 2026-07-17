using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs;
using ToDoListAPI.Models;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TodoController(AppDbContext appDbContext)
        {
            this._db = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(CreateTodoDto createTodoDto) 
        {
            var todo = new TodoItem { Title = createTodoDto.Title, UserId = createTodoDto.UserId, Status = Status.Pending };

            await _db.TodoItems.AddAsync(todo);
            await _db.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserTodos(int userId) 
        { 
            var todos = await _db.TodoItems
                .Where(t => t.UserId == userId)
                .Select(t => new GetTodoDto { Id = t.Id, UserId = t.UserId, Title = t.Title, Status = t.Status })
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet("{todoId}")]
        public async Task<IActionResult> GetTodoDetails(int todoId) 
        {
            var todo = await _db.TodoItems
                .Where(t => t.Id == todoId)
                .Select(t => new GetTodoDto { Id = t.Id, UserId = t.UserId, Title = t.Title, Status = t.Status})
                .FirstOrDefaultAsync();

            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPut("{todoId}")]
        public async Task<IActionResult> UpdateTodo(int todoId,  UpdateTodoDto updateTodoDto) 
        {
            var todo = await _db.TodoItems
                .FindAsync(todoId);

            if (todo == null) return NotFound();

            todo.Title = updateTodoDto.Title;
            todo.Status = updateTodoDto.Status;

            _db.TodoItems.Update(todo);
            await _db.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete("{todoId}")]
        public async Task<IActionResult> DeleteTodo(int todoId) 
        { 
            var todo = await _db.TodoItems.FindAsync(todoId);
            if (todo == null ) return NotFound();

            _db.TodoItems.Remove(todo);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTodosByStatus(Status status, [FromQuery] int userId) 
        {
            var todos = await _db.TodoItems
                .Where(t => t.Status == status && t.UserId == userId)
                .Select(t => new GetTodoDto { Id = t.Id, UserId = t.UserId, Title = t.Title, Status = t.Status})
                .ToListAsync();

            return Ok(todos);
        }

    }
}
