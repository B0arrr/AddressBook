using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models;

public class Book
{
    public Guid Id { get; init; }
    [Required]
    public City City { get; set; }
    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(40)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(80)]
    public string? CompanyName { get; set; }
}