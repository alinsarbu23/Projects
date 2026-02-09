using Insurance.Application.Interfaces.Repositories;

namespace Insurance.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InsuranceDbContext _context;

    public IClientRepository Clients { get; }

    public IBuildingRepository Buildings { get; }

    public IGeographyRepository Geography { get; }

    public IClientIdentifierRepository ClientIdentifiers { get; }

    public IBuildingTypeRepository BuildingTypes { get; }

    public ICityRepository Cities { get; }

    public IAdministratorRepository Administrators { get; }

    public IPolicyRepository Policies { get; }

    public IBrokerRepository Brokers { get; }

    public ICurrencyRepository Currencies { get; }

    public IFeeConfigurationRepository FeeConfigurations { get; }

    public IRiskFactorConfigurationRepository RiskFactorConfigurations { get; }

    public UnitOfWork(
        InsuranceDbContext context,
        IClientRepository clientRepository,
        IBuildingRepository buildingRepository,
        IGeographyRepository geographyRepository,
        IClientIdentifierRepository clientIdentifierRepository,
        IBuildingTypeRepository buildingTypes,
        ICityRepository cities,
        IAdministratorRepository administrators,
        IBrokerRepository brokers,
        ICurrencyRepository currencies,
        IFeeConfigurationRepository feeConfigurations,
        IPolicyRepository policies,
        IRiskFactorConfigurationRepository riskFactorConfigurations)
    {
        Clients = clientRepository;
        Buildings = buildingRepository;
        Geography = geographyRepository;
        ClientIdentifiers = clientIdentifierRepository;
        BuildingTypes = buildingTypes;
        Cities = cities;
        Administrators = administrators;
        Brokers = brokers;
        Currencies = currencies;
        FeeConfigurations = feeConfigurations;
        RiskFactorConfigurations = riskFactorConfigurations;
        Policies = policies;
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
