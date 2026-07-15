namespace ToDoListAPI.Models
{
    public enum Status { Pending, InProgress, Completed }
    public class TodoItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
    }
}
