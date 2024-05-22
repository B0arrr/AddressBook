using AddressBook.Models;

namespace AddressBook.DTOs;

public class BookDTO
{
    public Guid Id { get; init; }
    public City City { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
}