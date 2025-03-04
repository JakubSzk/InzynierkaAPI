using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Models
{
    public class ItemPhotoModel
    {
        public int Id { get; set; }
        public ItemModel Item { get; set; } = null!;
        public PhotoModel Photo { get; set; } = null!;
    }
}
