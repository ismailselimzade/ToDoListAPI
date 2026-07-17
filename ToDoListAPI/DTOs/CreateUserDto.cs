namespace ToDoListAPI.DTOs
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
