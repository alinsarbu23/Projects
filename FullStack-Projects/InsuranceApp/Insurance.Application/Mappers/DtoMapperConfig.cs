using AutoMapper;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.DTOs.Geography;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;

namespace Insurance.Application.Mappers;

public class DtoMapperConfig : Profile
{
    public DtoMapperConfig()
    {

        CreateMap<Client, GetClientByIdDto>()
            .ForMember(d => d.ClientType, o => o.Ignore());

        CreateMap<Client, GetClientsDto>()
            .ForMember(d => d.ClientType, o => o.Ignore());

        CreateMap<CreateClientDto, Client>()
            .ForMember(d => d.Id, o => o.Ignore());


        CreateMap<Building, ClientBuildingSummaryDto>()
            .ForMember(d => d.City, o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.BuildingType, o => o.MapFrom(s => s.BuildingType.Name));

        CreateMap<Building, GetBuildingByIdDto>()
            .ForMember(d => d.City, o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.County, o => o.MapFrom(s => s.City.County.Name))
            .ForMember(d => d.Country, o => o.MapFrom(s => s.City.County.Country.Name))
            .ForMember(d => d.BuildingType, o => o.MapFrom(s => s.BuildingType.Name));

        CreateMap<CreateBuildingDto, Building>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<UpdateBuildingDto, Building>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.ClientId, o => o.Ignore())
            .ForMember(d => d.CityId, o => o.Ignore())
            .ForMember(d => d.BuildingTypeId, o => o.Ignore());


        CreateMap<Policy, GetPolicyByIdDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s =>
                s.EndDate < DateTime.UtcNow
                    ? PolicyStatus.Expired.ToString()
                    : s.Status.ToString()
            ))
            .ForMember(d => d.CurrencyCode, o => o.Ignore())
            .ForMember(d => d.BuildingAddress,
                o => o.MapFrom(s => $"{s.Building.Street} {s.Building.StreetNumber}"));


        CreateMap<Policy, GetPoliciesDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s =>
                s.EndDate < DateTime.UtcNow
                    ? PolicyStatus.Expired.ToString()
                    : s.Status.ToString()
            ));



        CreateMap<Broker, GetBrokerByIdDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<Broker, GetBrokersDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<CreateBrokerDto, Broker>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore());

        CreateMap<UpdateBrokerDto, Broker>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Status, o => o.Ignore());


        CreateMap<Currency, GetCurrencyByIdDto>();
        CreateMap<Currency, GetCurrenciesDto>();

        CreateMap<CreateCurrencyDto, Currency>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<UpdateCurrencyDto, Currency>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<FeeConfiguration, GetFeeConfigurationByIdDto>();
        CreateMap<FeeConfiguration, GetFeeConfigurationsDto>();

        CreateMap<CreateFeeConfigurationDto, FeeConfiguration>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<UpdateFeeConfigurationDto, FeeConfiguration>()
            .ForMember(d => d.Id, o => o.Ignore());


        CreateMap<RiskFactorConfiguration, GetRiskFactorConfigurationByIdDto>();
        CreateMap<RiskFactorConfiguration, GetRiskFactorConfigurationsDto>();

        CreateMap<CreateRiskFactorConfigurationDto, RiskFactorConfiguration>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<UpdateRiskFactorConfigurationDto, RiskFactorConfiguration>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<Country, GetCountriesDto>();
        CreateMap<County, GetCountiesByCountryDto>();
        CreateMap<City, GetCitiesByCountyDto>();
    }
}
