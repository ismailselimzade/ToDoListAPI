namespace ToDoListAPI.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return password + "hashed";
        }
    }
}
