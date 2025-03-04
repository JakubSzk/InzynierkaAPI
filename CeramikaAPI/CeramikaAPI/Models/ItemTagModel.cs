using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    public class ItemTagModel
    {
        public int Id { get; set; }
        public ItemModel Item { get; set; } = null!;
        public TagModel Tag { get; set; } = null!;
    }
}
