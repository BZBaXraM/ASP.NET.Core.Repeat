using Crud.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Person> Persons => Set<Person>();
    public virtual DbSet<Country> Countries => Set<Country>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>().ToTable("Persons");
        modelBuilder.Entity<Country>().ToTable("Countries");

        modelBuilder.Entity<Country>()
            .HasData(new List<Country>
            {
                new() { CountryId = new Guid("18f29207-6401-45c2-8820-e400b1c14461"), CountryName = "Azerbaijan" },
                new() { CountryId = new Guid("272d4b54-ed10-4644-96cf-f0fce4b88bb1"), CountryName = "USA" },
                new() { CountryId = new Guid("49e9d52e-32ef-4fd3-b35f-c66df8a8aad3"), CountryName = "Australia" },
                new() { CountryId = new Guid("983bbc9d-3556-4942-8cbf-ad239c532ccd"), CountryName = "Canada" },
                new() { CountryId = new Guid("a77f3c7a-72f0-4646-83d3-bbac2128bff3"), CountryName = "UK" }
            });

        modelBuilder.Entity<Person>()
            .HasData(new List<Person>
            {
                new()
                {
                    PersonId = Guid.NewGuid(),
                    PersonName = "John Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1980, 1, 1), DateTimeKind.Utc),
                    Gender = "Male",
                    CountryId = new Guid("272d4b54-ed10-4644-96cf-f0fce4b88bb1"),
                    Address = "123 Main St, Anytown, USA",
                    ReceiveNewsLetters = false
                },
                new()
                {
                    PersonId = Guid.NewGuid(),
                    PersonName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 1, 1), DateTimeKind.Utc),
                    Gender = "Female",
                    CountryId = new Guid("a77f3c7a-72f0-4646-83d3-bbac2128bff3"),
                    Address = "456 Elm St, Othertown, USA",
                    ReceiveNewsLetters = false
                }
            });

        modelBuilder.Entity<Person>().Property(temp => temp.Tin)
            .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("varchar(8)")
            .HasDefaultValue("ABC12345");

        modelBuilder.Entity<Person>()
            .ToTable(t => t.HasCheckConstraint("CHK_TIN", "char_length(\"TaxIdentificationNumber\") = 8"));

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasOne<Country>(c => c.Country)
                .WithMany(p => p.Persons)
                .HasForeignKey(p => p.CountryId);
        });
    }
}