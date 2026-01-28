using AutoMapper;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Geography;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;

namespace Insurance.Application.Mappers;

public class DtoMapperConfig : Profile
{
    public DtoMapperConfig()
    {

        CreateMap<Client, GetClientsDto>();
        CreateMap<Client, GetClientByIdDto>();

        CreateMap<Building, ClientBuildingSummaryDto>()
            .ForMember(d => d.City,
                o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.BuildingType,
                o => o.MapFrom(s => s.BuildingType.Name));

        CreateMap<Building, GetBuildingByIdDto>()
            .ForMember(d => d.City,
                o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.County,
                o => o.MapFrom(s => s.City.County.Name))
            .ForMember(d => d.Country,
                o => o.MapFrom(s => s.City.County.Country.Name))
            .ForMember(d => d.BuildingType,
                o => o.MapFrom(s => s.BuildingType.Name));

        CreateMap<Country, GetCountriesDto>();
        CreateMap<County, GetCountiesByCountryDto>();
        CreateMap<City, GetCitiesByCountyDto>();
    }
}
