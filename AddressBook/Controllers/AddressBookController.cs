using AddressBook.DTOs;
using AddressBook.Interfaces;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

[ApiController]
[Route("addressBooks")]
public class AddressBookController(
    ILogger<AddressBookController> logger,
    ICRUDGeneric<Book, BookDTO> addressBookService,
    ICRUDGeneric<City, CityDTO> cityService) : ControllerBase
{
    [HttpGet("getAll", Name = "GetAddressBooks")]
    public async Task<IResult> GetAll()
    {
        var entities = (await addressBookService.GetAll()).ToList();
        logger.LogInformation("GetAll - {Count} entities", entities.Count);
        return Results.Ok(entities);
    }

    [HttpGet("getById/{id}", Name = "GetAddressBookById")]
    public async Task<IResult> GetById(int id)
    {
        var addressBook = await addressBookService.GetBy(x => x.Id == id);
        if (addressBook is null)
        {
            logger.LogInformation("GetById - entity with id: {Id} doesn't exist", id);
            return Results.NotFound($"AddressBook with id: {id} doesn't exists");
        }

        logger.LogInformation("GetById - entity exists");
        return Results.Ok(addressBook);
    }

    [HttpGet("getLast", Name = "GetLastAddressBook")]
    public async Task<IResult> GetLast()
    {
        var addressBook = (await addressBookService.GetAll()).ToList();
        if (!addressBook.Any())
        {
            logger.LogInformation("GetLast - there is no entities");
            return Results.NotFound("AddressBook is empty");
        }

        logger.LogInformation("GetLast - entity exists");
        return Results.Ok(addressBook.Last());
    }

    [HttpGet("getAllFromCity/{city}", Name = "GetAllAddressBookFromCity")]
    public async Task<IResult> GetAllFromCity(string city)
    {
        var cityInDb = (await cityService.GetBy(x => x.CityName == city));
        if (cityInDb is null)
        {
            logger.LogInformation("GetAllFromCity - City doesn't exists");
            return Results.NotFound("City doesn't exists");
        }

        var entities = (await addressBookService.GetAll(x => x.CityId == cityInDb.Id)).ToList();
        logger.LogInformation("GetAllFromCity - {Count} entities", entities.Count);
        return Results.Ok(entities);
    }

    [HttpPost("add", Name = "AddAddressBook")]
    public async Task<IResult> AddAddressBook(BookDTO book)
    {
        var city = await cityService.GetBy(x => x.Id == book.CityId);
        if (city is null)
        {
            logger.LogError("AddAddressBook - there is not city with id: {BookCityId}", book.CityId);
            return Results.NotFound($"City with id: {book.CityId} doesn't exists");
        }

        var result = await addressBookService.Add(book);
        if (result is null)
        {
            logger.LogError("AddAddressBook - AddressBook not added");
            return Results.Problem("AddressBook not added");
        }

        logger.LogInformation("AddAddressBook - AddressBook added");
        return Results.Ok(result);
    }

    [HttpPut("update/{id}", Name = "UpdateAddressBook")]
    public async Task<IResult> UpdateAddressBook(BookDTO book, int id)
    {
        var entity = await addressBookService.GetBy(x => x.Id == id);
        if (entity is null)
        {
            logger.LogError("UpdateAddressBook - AddressBook with id: {Id} doesn't exists", id);
            return Results.NotFound($"AddressBook with id: {id} doesn't exists");
        }

        var result = await addressBookService.Update(book, x => x.Id == id);
        if (result is null)
        {
            logger.LogError("UpdateAddressBook - AddressBook not updated");
            return Results.Problem("AddressBook not updated");
        }

        logger.LogInformation("UpdateAddressBook - AddressBook with id {Id} updated", id);
        return Results.Ok(result);
    }

    [HttpDelete("delete/{id}", Name = "DeleteAddressBook")]
    public async Task<IResult> DeleteAddressBook(int id)
    {
        var entity = await addressBookService.GetBy(x => x.Id == id);
        if (entity is null)
        {
            logger.LogError("DeleteAddressBook - AddressBook with id: {Id} doesn't exists", id);
            return Results.NotFound($"AddressBook with id: {id} doesn't exists");
        }

        var result = await addressBookService.Delete(id);
        if (result is null)
        {
            logger.LogError("DeleteAddressBook - AddressBook not deleted");
            return Results.Problem("AddressBook not deleted");
        }

        logger.LogInformation("DeleteAddressBook - AddressBook with id {Id} deleted", id);
        return Results.Ok(result);
    }
}