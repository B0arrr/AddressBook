using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Models;

public class Book
{
    public int Id { get; init; }
    public int CityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
    public City City { get; set; } = null!;
}