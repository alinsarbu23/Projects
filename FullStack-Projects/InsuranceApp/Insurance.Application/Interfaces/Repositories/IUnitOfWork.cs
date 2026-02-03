namespace Insurance.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IClientRepository Clients { get; }
    IBuildingRepository Buildings { get; }
    IGeographyRepository Geography { get; }
    IClientIdentifierRepository ClientIdentifiers { get; }
    IBuildingTypeRepository BuildingTypes { get; }
    ICityRepository Cities { get; }
    IAdministratorRepository Administrators { get; }
    IBrokerRepository Brokers { get; }
    IPolicyRepository Policies { get; }
    IRiskFactorConfigurationRepository RiskFactorConfigurations { get; }
    IFeeConfigurationRepository FeeConfigurations { get; }
    ICurrencyRepository Currencies { get; }

    Task<int> SaveChangesAsync();
}
