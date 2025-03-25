using AutoMapper;
using CarParkFinder.Domain.DTOs;
using CarParkFinder.Domain.Entities;

public class CarParkMappingProfile : Profile
{
    public CarParkMappingProfile()
    {
        // Map CarParkCreateDto → CarPark (Used for POST Requests)
        CreateMap<CarParkDto, CarPark>();

        // Map CarPark → CarParkResponseDto (Used for API Response)
        CreateMap<CarPark, CarParkResponseDto>();

        // Map CarPark → CarParkDto (Used for API Response)
        CreateMap<CarPark, CarParkDto>();
    }
}
