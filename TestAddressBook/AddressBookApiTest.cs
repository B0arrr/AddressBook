using AddressBook.Controllers;
using AddressBook.CRUD;
using AddressBook.DTOs;
using AddressBook.Models;
using AddressBook.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestAddressBook;

public class AddressBookApiTest(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>
{
    public IMapper Mapper { get; set; } = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>())
        .CreateMapper();

    private AddressBookController GetAddressBookController()
    {
        var loggerAddressBook = LoggerFactory.Create(opt => { }).CreateLogger<AddressBookController>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var addressBookService = new CRUDGeneric<Book, BookDTO>(databaseFixture.DbContext, mapper);
        var cityService = new CRUDGeneric<City, CityDTO>(databaseFixture.DbContext, mapper);

        return new AddressBookController(loggerAddressBook, addressBookService, cityService);
    }

    [Fact]
    public async void TestGetAllShouldReturnAllOfEntities()
    {
        var addressBookController = GetAddressBookController();
        var amount = databaseFixture.DbContext.Books?.Count();

        var result = await addressBookController.GetAll();

        var okResult = Assert.IsType<Ok<List<BookDTO>>>(result);
        var returnValue = Assert.IsType<List<BookDTO>>(okResult.Value);
        Assert.Equal(amount, returnValue.Count);
    }

    [Fact]
    public async void TestGetByIdShould404WhenAddressBookDoesntExists()
    {
        var data = databaseFixture.DbContext.Books?.ToList();
        foreach (var book in data)
        {
            databaseFixture.DbContext.Books?.Remove(book);
        }

        await databaseFixture.DbContext.SaveChangesAsync();

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetById(5);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal($"AddressBook with id: 5 doesn't exists", notFoundResult.Value);

        foreach (var book in data)
        {
            databaseFixture.DbContext.Books?.Add(book);
        }

        await databaseFixture.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async void TestGetByIdShouldReturn200WithAddressBook()
    {
        var obj = Mapper.Map<BookDTO>(databaseFixture.DbContext.Books?.FirstOrDefault(x => x.Id == 2));

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetById(2);

        var okResult = Assert.IsType<Ok<BookDTO>>(result);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(obj);

        Assert.Equal(jsonDto, jsonResult);
    }

    [Fact]
    public async void TestGetLastShould404WhenAddressBookIsEmpty()
    {
        var data = databaseFixture.DbContext.Books?.ToList();
        foreach (var book in data)
        {
            databaseFixture.DbContext.Books?.Remove(book);
        }

        await databaseFixture.DbContext.SaveChangesAsync();

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetLast();

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("AddressBook is empty", notFoundResult.Value);

        foreach (var book in data)
        {
            databaseFixture.DbContext.Books?.Add(book);
        }

        await databaseFixture.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async void TestGetLastShouldReturnLastEntity()
    {
        var last = databaseFixture.DbContext.Books?.ToList().Last();
        var lastDto = Mapper.Map<BookDTO>(last);

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetLast();

        var okResult = Assert.IsType<Ok<BookDTO>>(result);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(lastDto);

        Assert.Equal(jsonDto, jsonResult);
    }

    [Fact]
    public async void TestGetAllFromCityShould404WhenCityDoesntExists()
    {
        var data = databaseFixture.DbContext.Cities?.ToList();
        foreach (var city in data)
        {
            databaseFixture.DbContext.Cities?.Remove(city);
        }

        await databaseFixture.DbContext.SaveChangesAsync();

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetAllFromCity("test");

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("City doesn't exists", notFoundResult.Value);

        foreach (var city in data)
        {
            databaseFixture.DbContext.Cities?.Add(city);
        }

        await databaseFixture.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async void TestGetAllFromCityShouldReturnAllEntitiesFromCity()
    {
        var listDTO = Mapper.Map<List<BookDTO>>(databaseFixture.DbContext.Books?.Where(x => x.CityId == 1).ToList());
        var addressBookController = GetAddressBookController();

        var result = await addressBookController.GetAllFromCity("Bydgoszcz");

        var okResult = Assert.IsType<Ok<List<BookDTO>>>(result);
        Assert.Equal(200, okResult.StatusCode);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(listDTO);

        Assert.Equal(jsonDto, jsonResult);
    }

    [Fact]
    public async void TestAddAddressBookShould404WhenCityDoesntExists()
    {
        var bookDTO = new BookDTO
        {
            Id = 4,
            CityId = 1,
            FirstName = "Grzegorz",
            LastName = "Kieczka"
        };

        var data = databaseFixture.DbContext.Cities?.ToList();
        foreach (var city in data)
        {
            databaseFixture.DbContext.Cities?.Remove(city);
        }

        await databaseFixture.DbContext.SaveChangesAsync();

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.AddAddressBook(bookDTO);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal($"City with id: 1 doesn't exists", notFoundResult.Value);

        foreach (var city in data)
        {
            databaseFixture.DbContext.Cities?.Add(city);
        }

        await databaseFixture.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async void TestAddAddressBookShouldAddAddressBook()
    {
        var bookDTO = new BookDTO
        {
            Id = 4,
            CityId = 1,
            FirstName = "Grzegorz",
            LastName = "Kieczka"
        };

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.AddAddressBook(bookDTO);

        var okResult = Assert.IsType<Ok<BookDTO>>(result);
        Assert.Equal(200, okResult.StatusCode);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(bookDTO);

        Assert.Equal(jsonDto, jsonResult);
    }

    [Fact]
    public async void TestUpdateAddressBookShould404WhenAddressBookDoesntExists()
    {
        var bookDTO = new BookDTO
        {
            Id = 4,
            CityId = 1,
            FirstName = "Grzegorz",
            LastName = "Kieczka"
        };

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.UpdateAddressBook(bookDTO, 5);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal($"AddressBook with id: 5 doesn't exists", notFoundResult.Value);
    }

    [Fact]
    public async void TestUpdateAddressBookShouldUpdateAddressBook()
    {
        var bookDTO = new BookDTO
        {
            Id = 1,
            CityId = 1,
            FirstName = "Jan",
            LastName = "Kowalski"
        };

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.UpdateAddressBook(bookDTO, 1);

        var okResult = Assert.IsType<Ok<BookDTO>>(result);
        Assert.Equal(200, okResult.StatusCode);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(bookDTO);

        Assert.Equal(jsonDto, jsonResult);
    }

    [Fact]
    public async void TestDeleteAddressBookShould404WhenAddressBookDoesntExists()
    {
        var bookDTO = new BookDTO
        {
            Id = 4,
            CityId = 1,
            FirstName = "Grzegorz",
            LastName = "Kieczka"
        };

        var addressBookController = GetAddressBookController();

        var result = await addressBookController.DeleteAddressBook(5);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal($"AddressBook with id: 5 doesn't exists", notFoundResult.Value);
    }

    [Fact]
    public async void TestDeleteAddressBookShouldDeleteAddressBook()
    {
        var bookDTO = Mapper.Map<BookDTO>(databaseFixture.DbContext.Books?.Last());
        
        var addressBookController = GetAddressBookController();

        int id = bookDTO.Id is int ? (int)bookDTO.Id : 0;

        var result = await addressBookController.DeleteAddressBook(id);

        var okResult = Assert.IsType<Ok<BookDTO>>(result);
        Assert.Equal(200, okResult.StatusCode);

        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var jsonDto = JsonConvert.SerializeObject(bookDTO);

        Assert.Equal(jsonDto, jsonResult);
    }
}