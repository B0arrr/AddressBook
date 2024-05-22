using System.ComponentModel.DataAnnotations;

namespace AddressBook.Migrations;

public class City
{
    public int Id { get; init; }
    [Required]
    [MaxLength(30)]
    public string CityName { get; set; }
}