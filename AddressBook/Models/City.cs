using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models;

public class City
{
    public int Id { get; init; }
    public string CityName { get; set; }
    public ICollection<Book> Books { get; } = new List<Book>();
}