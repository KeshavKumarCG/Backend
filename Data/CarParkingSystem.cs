using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class CarParkingContext : DbContext
    {
        public CarParkingContext(DbContextOptions<CarParkingContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarStatus> CarStatus { get; set; }
        public DbSet<CarStatusLog> CarStatusLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
