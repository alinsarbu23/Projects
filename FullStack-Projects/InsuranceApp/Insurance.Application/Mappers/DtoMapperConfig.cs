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

        CreateMap<Building, ClientBuildingSummaryDto>();
        CreateMap<Building, GetBuildingByIdDto>();

        CreateMap<Country, GetCountriesDto>();
        CreateMap<County, GetCountiesByCountryDto>();
        CreateMap<City, GetCitiesByCountyDto>();
    }
}
