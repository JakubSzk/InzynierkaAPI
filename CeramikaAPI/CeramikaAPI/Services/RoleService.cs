using CeramikaAPI.Context;
using CeramikaAPI.Models;

namespace CeramikaAPI.Services
{
    public class RoleService
    {
        private CeramikaContext context;
        public RoleService() { context = new CeramikaContext(); }

        public RoleModel? AddRole(RoleModel role)
        {
            context.Roles.Add(role);
            try { context.SaveChanges(); } catch { return null; }
            return role;
        }

        public RoleModel? roleById(int id)
        {
            try
            {
                return context.Roles.First(x => x.Id == id);
            }
            catch { return null; }
        }

        public UserRolesModelDTO? SetRole(int roleId, int userId)
        { 
            UserService userservice = new UserService();
            UserModel user = userservice.UserById(userId);
            RoleModel role = roleById(roleId);
            context.Attach(user);
            context.Attach(role);
            context.Add( new UserRolesModel { Role = role, User = user });
            try { context.SaveChanges(); } catch { return null;}
            return new UserRolesModelDTO { User = user.Name, Role = role.Name };
        }

        public List<UserRolesModelDTO>? GetUserRoles()
        {
            try
            {
                return context.UserRoles.Select(c => new UserRolesModelDTO
                {
                    Role = c.Role.Name,
                    User = c.User.Name
                }).ToList();
            } catch { return null; }
        }

        public List<RoleModel>? GetRoles()
        {
            try { return context.Roles.ToList(); } catch { return null; }
        }
    }
}
