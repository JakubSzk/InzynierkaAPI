namespace CeramikaAPI.Models
{
    public class ItemListModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public List<string> Tags { get; set; }
        public string Author { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public int Avaible {  get; set; }
    }
}
