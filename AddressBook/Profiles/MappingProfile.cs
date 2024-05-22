using AddressBook.DTOs;
using AddressBook.Models;
using AutoMapper;

namespace AddressBook.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDTO>();
        CreateMap<BookDTO, Book>();
        CreateMap<City, CityDTO>();
        CreateMap<CityDTO, City>();
    }
}