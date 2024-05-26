using AddressBook.Models;

namespace AddressBook.DTOs;

public class BookDTO
{
    public int? Id { get; init; }
    public int CityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
}