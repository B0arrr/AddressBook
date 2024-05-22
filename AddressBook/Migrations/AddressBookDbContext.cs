using Microsoft.EntityFrameworkCore;

namespace AddressBook.Migrations;

public class AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : DbContext(options)
{
    public DbSet<Book>? Books { get; set; }
    public DbSet<City>? Cities { get; set; }
}