namespace ToDoListAPI.Models
{
    public class User
    {
        public User ()
        {
            TodoItems = new HashSet<TodoItem>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }
    }
}
