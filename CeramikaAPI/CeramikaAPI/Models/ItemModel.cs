using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Avaible {  get; set; }
        public float Price { get; set; }
        public string Model { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}
