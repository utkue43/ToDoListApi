using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users user, Dictionary<string, string> users);
        object Authenticate(Users userdata);
    }
}
