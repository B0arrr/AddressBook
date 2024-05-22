using AddressBook.DTOs;
using AddressBook.Interfaces;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressBookController(
    ILogger<AddressBookController> logger,
    ICRUDGeneric<Book, BookDTO> addressBookService) : ControllerBase
{
    private readonly ILogger<AddressBookController> _logger = logger;

    [HttpGet(Name = "GetAddressBooks")]
    public async Task<IEnumerable<BookDTO>> GetAll()
    {
        return await addressBookService.GetAll();
    }
}