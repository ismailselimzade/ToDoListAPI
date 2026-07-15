using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Models;

namespace ToDoListAPI.Data
{
    public class AppDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<TodoItem> TodoItems { get; set; }

        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
