using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class CourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Private { get; set; }
        public int Taken { get; set; }
        public int Seats { get; set; }
        public DateTime When { get; set; }
        public UserModel Teacher { get; set; } = null!;
    }
}
