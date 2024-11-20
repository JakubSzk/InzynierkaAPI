using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CeramikaAPI.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Name), IsUnique = true)]
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}
