using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
