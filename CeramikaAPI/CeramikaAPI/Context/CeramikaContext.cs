using CeramikaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CeramikaAPI.Context
{
    public class CeramikaContext :DbContext
    {
        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<CourseModel> Courses { get; set; } = null!;
        public DbSet<RoleModel> Roles { get; set; } = null!;
        public DbSet<SignedForModel> SignedFor { get; set; } = null!;
        public DbSet<UserRolesModel> UserRoles { get; set; } = null!;
        public DbSet<ItemModel> Items { get; set; } = null!;
        public DbSet<PhotoModel> Photos { get; set; } = null!;
        public DbSet<TagModel> Tags { get; set; } = null!;
        public DbSet<ItemPhotoModel> ItemPhotos { get; set; } = null!;
        public DbSet<ItemTagModel> ItemTags { get; set; } = null!;

        private string DbPath;
        public CeramikaContext()
        {
            DbPath = "sqlite.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

    }
}
