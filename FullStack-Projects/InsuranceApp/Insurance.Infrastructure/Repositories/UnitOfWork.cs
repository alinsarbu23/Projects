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


    public UnitOfWork(
        InsuranceDbContext context, 
        IClientRepository clientRepository, 
        IBuildingRepository buildingRepository,
        IGeographyRepository geographyRepository,
        IClientIdentifierRepository clientIdentifierRepository,
        IBuildingTypeRepository buildingTypes,
        ICityRepository cities)
    {
        Clients = clientRepository;
        Buildings = buildingRepository;
        Geography = geographyRepository;
        ClientIdentifiers = clientIdentifierRepository;
        BuildingTypes = buildingTypes;
        Cities = cities;
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
