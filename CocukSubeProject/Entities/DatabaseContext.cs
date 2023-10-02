using Microsoft.EntityFrameworkCore;

namespace CocukSubeProject.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
    }
}
