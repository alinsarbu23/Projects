using AutoMapper;
using Insurance.Domain.Enums;
using DaoAdmin = Insurance.Infrastructure.DAOs.Administrator;
using DaoBroker = Insurance.Infrastructure.DAOs.Broker;
using DaoBuilding = Insurance.Infrastructure.DAOs.Building;
using DaoBuildingType = Insurance.Infrastructure.DAOs.BuildingType;
using DaoCity = Insurance.Infrastructure.DAOs.City;
using DaoClient = Insurance.Infrastructure.DAOs.Client;
using DaoClientIdentifier = Insurance.Infrastructure.DAOs.ClientIdentifier;
using DaoClientType = Insurance.Infrastructure.DAOs.ClientType;
using DaoCountry = Insurance.Infrastructure.DAOs.Country;
using DaoCounty = Insurance.Infrastructure.DAOs.County;
using DaoCurrency = Insurance.Infrastructure.DAOs.Currency;
using DaoFeeConfiguration = Insurance.Infrastructure.DAOs.FeeConfiguration;
using DaoPolicy = Insurance.Infrastructure.DAOs.Policy;
using DaoRiskFactorConfiguration = Insurance.Infrastructure.DAOs.RiskFactorConfiguration;

using DomainAdmin = Insurance.Domain.Models.Administrator;
using DomainBroker = Insurance.Domain.Models.Broker;
using DomainBuilding = Insurance.Domain.Models.Building;
using DomainBuildingType = Insurance.Domain.Models.BuildingType;
using DomainCity = Insurance.Domain.Models.Geography.City;
using DomainClient = Insurance.Domain.Models.Client;
using DomainClientIdentifier = Insurance.Domain.Models.ClientIdentifier;
using DomainClientType = Insurance.Domain.Models.ClientType;
using DomainCountry = Insurance.Domain.Models.Geography.Country;
using DomainCounty = Insurance.Domain.Models.Geography.County;
using DomainCurrency = Insurance.Domain.Models.Currency;
using DomainFeeConfiguration = Insurance.Domain.Models.FeeConfiguration;
using DomainPolicy = Insurance.Domain.Models.Policy;
using DomainRiskFactorConfiguration = Insurance.Domain.Models.RiskFactorConfiguration;

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

        CreateMap<DaoAdmin, DomainAdmin>().ReverseMap();

        CreateMap<DaoBroker, DomainBroker>().ReverseMap();

        CreateMap<DaoPolicy, DomainPolicy>().ReverseMap();

        CreateMap<DaoRiskFactorConfiguration, DomainRiskFactorConfiguration>()
            .ForMember(d => d.Level,
                o => o.MapFrom(s => Enum.Parse<RiskLevel>(s.Level)));

        CreateMap<DomainRiskFactorConfiguration, DaoRiskFactorConfiguration>()
            .ForMember(d => d.Level,
                o => o.MapFrom(s => s.Level.ToString()));

        CreateMap<DaoFeeConfiguration, DomainFeeConfiguration>()
            .ForMember(d => d.Type,
                o => o.MapFrom(s => Enum.Parse<FeeType>(s.Type)))
            .ForMember(d => d.AppliesToRisk,
                o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.AppliesToRisk)
                        ? (BuildingRiskType?)null
                        : Enum.Parse<BuildingRiskType>(s.AppliesToRisk)));

        CreateMap<DomainFeeConfiguration, DaoFeeConfiguration>()
            .ForMember(d => d.Type,
                o => o.MapFrom(s => s.Type.ToString()))
            .ForMember(d => d.AppliesToRisk,
                o => o.MapFrom(s =>
                    s.AppliesToRisk.HasValue
                        ? s.AppliesToRisk.Value.ToString()
                        : null));

        CreateMap<DaoCurrency, DomainCurrency>().ReverseMap();
    }
}
