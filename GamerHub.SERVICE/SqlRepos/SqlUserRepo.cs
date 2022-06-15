using GamerHub.CORE.Models;
using GamerHub.CORE.WrapperModels;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.IRepos;
using GamerHub.SERVICE.Security;
using GamerHub.SERVICE.Validations;
using Microsoft.EntityFrameworkCore;

namespace GamerHub.SERVICE.SqlRepos
{
    public class SqlUserRepo : IUserRepo, IAdminRepo
    {
        private readonly GamerHubDBContext db_Context;

        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        private UserRegisterValidation userValidation = new();


        public SqlUserRepo(GamerHubDBContext db, JwtAuthenticationManager jwtAuthenticationManager)
        {
            this.db_Context = db;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public bool CheckUserWithEmail(string email)
        {
            if (db_Context.User.FirstOrDefault(p => p.Email == email) != null)
                return false;

            return true;
        }

        public bool CreateUser(User user)
        {
            if (userValidation.IsNameValid(user.Name) && userValidation.IsEmailValid(user.Email))
            {
                User encryptedUser = new User()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = Encryption.Encrypt(user.Password),
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                };
                db_Context.User.Add(encryptedUser);
                db_Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var user = GetUserById(id);
                UpdateUser(user);
                db_Context.User.Remove(user);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db_Context.User.ToList();
        }

        public IEnumerable<Friend> SearchUser(string search)
        {
            try
            {
                List<Friend> friendSearchlist = new List<Friend>();
                var x = db_Context.User.Where(p => p.Name.Contains(search));
                foreach (var user in x)
                {
                    Friend friend = new Friend()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Gender = user.Gender,
                        BirthDate = user.BirthDate
                    };
                    friendSearchlist.Add(friend);
                }

                return friendSearchlist;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        public UserClient UserLogin(User userLoginRequest)
        {
            User encryptedUser = db_Context.User.FirstOrDefault(p => p.Email == userLoginRequest.Email);
            if (encryptedUser != null)
            {
                if (Encryption.Decrypt(encryptedUser.Password) == userLoginRequest.Password)
                {
                    UserClient userClient = new UserClient();
                    userClient.Email = encryptedUser.Email;
                    userClient.Name = encryptedUser.Name;
                    userClient.Id = encryptedUser.Id;
                    userClient.Token = jwtAuthenticationManager.Authenticate(encryptedUser.Name);
                    return userClient;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public User GetUserById(int id)
        {
            var user = db_Context.User
                .Include(user => user.ReceivedFriendships)
                .Include(user => user.SentFriendships)
                   .FirstOrDefault(p => p.Id == id);

            return user;
        }

        public User GetOnlyUserById(int id)
        {
            var user = db_Context.User.FirstOrDefault(p => p.Id == id);
            return user;
        }

        public bool UpdateUser(User user)
        {
            try
            {
                db_Context.Entry(user).State = EntityState.Modified;
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw new ArgumentNullException(nameof(user));
            }
        }

        public bool CreateAdmin(Admin admin)
        {
            if (userValidation.IsNameValid(admin.Name) && userValidation.IsEmailValid(admin.Email))
            {
                db_Context.Admin.Add(admin);
                db_Context.SaveChanges();
                return true;
            }
            return false;
        }

        public AdminClient AdminLogin(Admin adminLoginRequest)
        {
            Admin encryptedAdmin = db_Context.Admin.FirstOrDefault(p => p.Email == adminLoginRequest.Email);
            if (encryptedAdmin != null)
            {
                if (/*Encryption.Decrypt(encryptedAdmin.Password)*/ encryptedAdmin.Password == adminLoginRequest.Password)
                {
                    AdminClient adminClient = new AdminClient();
                    adminClient.Email = encryptedAdmin.Email;
                    adminClient.Name = encryptedAdmin.Name;
                    adminClient.Id = Convert.ToString(encryptedAdmin.Id);
                    adminClient.Token = jwtAuthenticationManager.Authenticate(encryptedAdmin.Name);
                    return adminClient;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool UpdateAdmin(Admin admin)
        {
            try
            {
                db_Context.Entry(admin).State = EntityState.Modified;
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw new ArgumentNullException(nameof(admin));
            }
        }

        public bool DeleteAdmin(int id)
        {
            try
            {
                var admin = db_Context.Admin.Find(id);
                db_Context.Admin.Remove(admin);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<Admin> GetAllAdmins()
        {
            return db_Context.Admin.ToList();
        }

        public bool CheckAdminWithEmail(string email)
        {
            if (db_Context.Admin.FirstOrDefault(p => p.Email == email) != null)
                return false;

            return true;
        }
    }
}

