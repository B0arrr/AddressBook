using AddressBook.DTOs;
using AddressBook.Interfaces;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

[ApiController]
[Route("city")]
public class CityController(
    ILogger<CityController> logger,
    ICRUDGeneric<City, CityDTO> cityService,
    ICRUDGeneric<Book, BookDTO> addressBookService) : ControllerBase
{
    [HttpGet("getAll", Name = "GetAllCities")]
    public async Task<IResult> GetAllCities()
    {
        var entities = (await cityService.GetAll()).ToList();
        logger.LogInformation("GetAll - {Count} Cities", entities.Count);
        return Results.Ok(entities);
    }

    [HttpGet("getById/{id}", Name = "GetCityById")]
    public async Task<IResult> GetById(int id)
    {
        var city = await cityService.GetBy(x => x.Id == id);
        if (city is null)
        {
            logger.LogInformation("GetById - City with id: {Id} doesn't exist", id);
            return Results.NotFound($"City with id: {id} doesn't exists");
        }

        logger.LogInformation("GetById - City exists");
        return Results.Ok(city);
    }

    [HttpPost("add", Name = "AddCity")]
    public async Task<IResult> AddCity(CityDTO city)
    {
        var cityInDb = await cityService.GetBy(x => x.CityName == city.CityName);
        if (cityInDb is not null)
        {
            logger.LogError("AddCity - City with this name already exists");
            return Results.Conflict("City already exists");
        }

        var result = await cityService.Add(city);
        if (result is null)
        {
            logger.LogError("AddCity - City not added");
            return Results.NotFound("City not added");
        }

        logger.LogInformation("AddCity - City added");
        return Results.Ok(result);
    }

    [HttpPut("update/{id}", Name = "UpdateCity")]
    public async Task<IResult> UpdateCity(CityDTO city, int id)
    {
        var entity = await cityService.GetBy(x => x.Id == id);
        if (entity is null)
        {
            logger.LogError("UpdateCity - City with id: {Id} doesn't exists", id);
            return Results.NotFound($"City with id: {id} doesn't exists");
        }

        var result = await cityService.Update(city, x => x.Id == id);
        if (result is null)
        {
            logger.LogError("UpdateCity - City not updated");
            return Results.NotFound("City not updated");
        }

        logger.LogInformation("UpdateCity - City with id {Id} updated", id);
        return Results.Ok(city);
    }

    [HttpDelete("delete/{id}", Name = "DeleteCity")]
    public async Task<IResult> DeleteCity(int id)
    {
        var entity = await cityService.GetBy(x => x.Id == id);
        if (entity is null)
        {
            logger.LogError("DeleteCity - City with id: {Id} doesn't exists", id);
            return Results.NotFound($"City with id: {id} doesn't exists");
        }

        var addressBooks = (await addressBookService.GetAll(x => x.CityId == id)).ToList();
        if (addressBooks.Count != 0)
        {
            logger.LogError("DeleteCity - City not deleted");
            return Results.NotFound("Some AddressBooks uses this city");
        }

        var result = await cityService.Delete(id);
        if (result is null)
        {
            logger.LogError("DeleteCity - City not deleted");
            return Results.NotFound("City not deleted");
        }

        logger.LogInformation("DeleteCity - City with id {Id} deleted", id);
        return Results.Ok(result);
    }
}