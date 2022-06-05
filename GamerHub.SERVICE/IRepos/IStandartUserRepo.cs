using GamerHub.CORE.Models;
using GamerHub.CORE.WrapperModels;

namespace GamerHub.SERVICE.IRepos
{
    public interface IStandartUserRepo
    {
        bool CheckUserWithEmail(string mail);
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        IEnumerable<User> SearchUser(string search);
        bool CreateUser(User user);
        UserClient UserLogin(User userLoginRequest);
        bool UpdateUser(User user);
        bool DeleteUser(int id);
    }
}
