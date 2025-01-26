using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CeramikaAPI.Models
{
    [Index(nameof(When), IsUnique = true)]
    public class CourseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        public bool Private { get; set; }
        public int Taken { get; set; }
        [Required]
        public int Seats { get; set; }
        [Required]
        public int Length { get; set; }
        [Required]
        public DateTime When { get; set; }
        [Required]
        public UserModel Teacher { get; set; } = null!;
        public String Picture { get; set; } = null!;
    }
}
