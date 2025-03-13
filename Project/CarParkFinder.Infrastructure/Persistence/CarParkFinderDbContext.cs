using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarParkFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarParkFinder.Infrastructure.Persistence
{
    public class CarParkFinderDbContext : DbContext
    {
        public CarParkFinderDbContext(DbContextOptions<CarParkFinderDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarPark> CarParks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarParkFinderDbContext).Assembly);

            modelBuilder.Entity<CarPark>().HasKey(c => c.car_park_no);
        }
    }
}
