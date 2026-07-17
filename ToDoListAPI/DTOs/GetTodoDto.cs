using ToDoListAPI.Models;

namespace ToDoListAPI.DTOs
{
    public class GetTodoDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
    }
}
