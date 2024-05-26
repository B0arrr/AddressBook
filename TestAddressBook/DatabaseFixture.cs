using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace TestAddressBook;

public class DatabaseFixture : IDisposable
{
    public AddressBookDbContext DbContext { get; private set; }
    private Book[] _initialBooks;
    private City[] _initialCities;

    public DatabaseFixture()
    {
        _initialBooks = getInitialBookEntries();
        _initialCities = getInitialCityEntries();
        seedDatabase();
    }

    private void seedDatabase()
    {
        var options = getContextOptions();
        DbContext = new AddressBookDbContext(options);

        foreach (var book in _initialBooks)
        {
            DbContext.Books.Add(book);
        }

        foreach (var city in _initialCities)
        {
            DbContext.Cities.Add(city);
        }

        DbContext.SaveChanges();
    }

    private City[] getInitialCityEntries()
    {
        return
        [
            new City { Id = 1, CityName = "Bydgoszcz" },
            new City { Id = 2, CityName = "Władywostok" },
        ];
    }

    private Book[] getInitialBookEntries()
    {
        return
        [
            new Book { Id = 1, CityId = 1, FirstName = "Jan", LastName = "Kowalski" },
            new Book { Id = 2, CityId = 1, FirstName = "Wojciech", LastName = "Nowak" },
            new Book { Id = 3, CityId = 2, FirstName = "Krzysztof", LastName = "Małysz" }
        ];
    }

    public DbContextOptions<AddressBookDbContext> getContextOptions()
    {
        return new DbContextOptionsBuilder<AddressBookDbContext>().UseInMemoryDatabase("AddressBook").Options;
    }

    public void Dispose()
    {
        var options = getContextOptions();
        DbContext.Books.RemoveRange(_initialBooks);
        DbContext.Cities.RemoveRange(_initialCities);
    }
}