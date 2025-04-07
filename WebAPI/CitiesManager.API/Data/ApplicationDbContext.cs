using CitiesManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>()
            .HasData(new
            {
                CityId = Guid.Parse("d2b889d1-5c3e-4b8e-9b8e-1b2b3c4d5e6f"),
                CityName = "New York",
            });

        modelBuilder.Entity<City>()
            .HasData(new
            {
                CityId = Guid.Parse("e3c889d1-6d4e-5c8e-9c8e-2b3b4c5d6e7f"),
                CityName = "Los Angeles",
            });

        modelBuilder.Entity<City>()
            .HasData(new
            {
                CityId = Guid.Parse("f4d889d1-7e5e-6d9e-9d9e-3b4b5c6d7e8f"),
                CityName = "Chicago",
            });
    }
}