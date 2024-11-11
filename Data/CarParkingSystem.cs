using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class CarParkingSystem : DbContext
    {
        public CarParkingSystem(DbContextOptions<CarParkingSystem> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarStatus> CarStatus { get; set; }
        public DbSet<CarStatusLog> CarStatusLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarStatusLog>()
                .HasKey(c => c.LogID);  // Define LogID as the primary key

            modelBuilder.Entity<CarStatusLog>()
                .HasOne(c => c.User)
                .WithMany()  // Assuming one-to-many relationship with User
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<CarStatusLog>()
                .HasOne(c => c.Car)
                .WithMany()  // Assuming one-to-many relationship with Car
                .HasForeignKey(c => c.CarID);

            modelBuilder.Entity<CarStatusLog>()
                .HasOne(c => c.CarStatus)
                .WithMany()  // Assuming one-to-many relationship with CarStatus
                .HasForeignKey(c => c.StatusID);
        }

    }
}
