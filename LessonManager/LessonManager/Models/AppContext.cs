using System.Data.Entity;

namespace LessonManager.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TeachClass> Classes { get; set; }
        public DbSet<Dilosi> Dilosis { get; set; }
    }
}