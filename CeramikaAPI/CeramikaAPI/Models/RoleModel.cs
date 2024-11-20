using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
