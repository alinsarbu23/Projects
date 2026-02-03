using AutoMapper;

using DomainClient = Insurance.Domain.Models.Client;
using DomainBuilding = Insurance.Domain.Models.Building;
using DomainCountry = Insurance.Domain.Models.Geography.Country;
using DomainCounty = Insurance.Domain.Models.Geography.County;
using DomainCity = Insurance.Domain.Models.Geography.City;
using DomainClientIdentifier = Insurance.Domain.Models.ClientIdentifier;
using DomainBuildingType = Insurance.Domain.Models.BuildingType;
using DomainClientType = Insurance.Domain.Models.ClientType;

using DaoClientType = Insurance.Infrastructure.DAOs.ClientType;
using DaoBuildingType = Insurance.Infrastructure.DAOs.BuildingType;
using DaoClient = Insurance.Infrastructure.DAOs.Client;
using DaoBuilding = Insurance.Infrastructure.DAOs.Building;
using DaoCountry = Insurance.Infrastructure.DAOs.Country;
using DaoCounty = Insurance.Infrastructure.DAOs.County;
using DaoCity = Insurance.Infrastructure.DAOs.City;
using DaoClientIdentifier = Insurance.Infrastructure.DAOs.ClientIdentifier;

namespace Insurance.Infrastructure.Mappers;

public class DaoMapperConfig : Profile
{
    public DaoMapperConfig()
    {
        CreateMap<DaoCountry, DomainCountry>().ReverseMap();
        CreateMap<DaoCounty, DomainCounty>().ReverseMap();
        CreateMap<DaoCity, DomainCity>().ReverseMap();

        CreateMap<DaoClient, DomainClient>().ReverseMap();
        CreateMap<DaoClientIdentifier, DomainClientIdentifier>().ReverseMap();

        CreateMap<DaoBuilding, DomainBuilding>().ReverseMap();
        CreateMap<DaoBuildingType, DomainBuildingType>().ReverseMap();
        CreateMap<DaoClientType, DomainClientType>().ReverseMap();
    }
}
