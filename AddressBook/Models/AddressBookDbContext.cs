using AddressBook.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Models;

public class AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : DbContext(options), IContext
{
    public DbSet<Book>? Books { get; set; }
    public DbSet<City>? Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<Book>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .HasOne(e => e.City)
            .WithMany(e => e.Books)
            .HasForeignKey(e => e.CityId)
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .Property(x => x.FirstName)
            .HasMaxLength(20)
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .Property(x => x.LastName)
            .HasMaxLength(40)
            .IsRequired();
        
        modelBuilder.Entity<Book>()
            .Property(x => x.CompanyName)
            .HasMaxLength(80);
        
        modelBuilder.Entity<City>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<City>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        modelBuilder.Entity<City>()
            .Property(x => x.CityName)
            .HasMaxLength(30)
            .IsRequired();
    }
}