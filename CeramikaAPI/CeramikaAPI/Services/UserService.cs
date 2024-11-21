using CeramikaAPI.Context;
using CeramikaAPI.Models;

namespace CeramikaAPI.Services
{
    public class UserService
    {
        private CeramikaContext context;
        public UserService() { context = new CeramikaContext(); }
        public List<UserModel> GetUsers()
        {
            return context.Users.ToList();
        }

        private UserModel? UserByName(string name)
        { 
            try
            {
                return context.Users.First(x => x.Name == name);
            }
            catch { return null; }
        }

        private UserModel? UserById(int id)
        {
            try
            {
                return context.Users.First(x => x.Id == id);
            }
            catch { return null; }
        }

        private UserModel? UserByEmail(string email)
        {
            try
            {
                return context.Users.First(x => x.Email == email);
            }
            catch { return null; }
        }

        public UserModel? AddUser(UserModel user)
        {
            context.Users.Add(user);
            try { context.SaveChanges(); } catch { return null; }
            return user;
        }

        public UserModel? LogUser(string name, string password)
        {
            var user = UserByName(name);
            if (user == null) { return null; }
            if (user.Password != password) { return null; }
            return user;
        }

        public UserModel? UpdateUser(string name, string password, string? newName = "", string? newPassword = "", string? newEmail = "")
        {
            UserModel? user = LogUser(name, password);
            if (user == null) { return null; }
            if (!string.IsNullOrEmpty(newName)) { user.Name = newName; }
            if (!string.IsNullOrEmpty(newPassword)) { user.Password = newPassword; }
            if (!string.IsNullOrEmpty(newEmail)) { user.Email = newEmail; }
            context.Users.Update(user);
            try
            { context.SaveChanges(); }
            catch { return null; }
            return user;
        }

        public UserModel? DeleteUser(string name, string password)
        {
            var user = LogUser(name, password);
            if (user == null) { return null; }
            context.Users.Remove(user);
            try { context.SaveChanges(); } catch { return null; }
            return user;
        }
    }
}
