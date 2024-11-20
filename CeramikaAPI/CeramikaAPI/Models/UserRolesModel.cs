namespace CeramikaAPI.Models
{
    public class UserRolesModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; } = null!;
        public RoleModel Role { get; set; } = null!;
    }
}
