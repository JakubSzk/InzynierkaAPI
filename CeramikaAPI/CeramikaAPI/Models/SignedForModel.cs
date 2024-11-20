namespace CeramikaAPI.Models
{
    public class SignedForModel
    {
        public int Id { get; set; }
        public CourseModel Course { get; set; } = null!;
        public UserModel User { get; set; } = null!;
    }
}
