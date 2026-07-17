using ToDoListAPI.Models;

namespace ToDoListAPI.DTOs
{
    public class UpdateTodoDto
    {
        public string Title { get; set; }
        public Status Status { get; set; }
    }
}
