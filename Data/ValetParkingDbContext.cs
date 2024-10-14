using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Backend.Data
{
    public class ValetParkingDbContext : DbContext
    {
        public ValetParkingDbContext(DbContextOptions<ValetParkingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        
    }
}
