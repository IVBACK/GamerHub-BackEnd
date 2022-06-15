using GamerHub.CORE.Models;

namespace GamerHub.SERVICE.IRepos
{
    public interface IAdminRepo
    {
        bool CreateAdmin(Admin admin);
        bool UpdateAdmin(Admin admin);
        bool DeleteAdmin(int id);
        IEnumerable<Admin> GetAllAdmins();
    }
}
