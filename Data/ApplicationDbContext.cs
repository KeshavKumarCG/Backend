using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Archive> Archives { get; set; }

        // Remove UserArchiveModel if not persisting in the database
        // public DbSet<UserArchiveModel> UserArchiveModels { get; set; }
    }
}
