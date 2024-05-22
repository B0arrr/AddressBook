using AddressBook.Interfaces;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Migrations;

public class AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : DbContext(options), IContext
{
    public DbSet<Book>? Books { get; set; }
    public DbSet<City>? Cities { get; set; }
}