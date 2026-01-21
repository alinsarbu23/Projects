using Insurance.Application.Interfaces.Repositories;

namespace Insurance.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InsuranceDbContext _context;
    public IClientRepository Clients { get; }
    public IBuildingRepository Buildings { get; }
    public IGeographyRepository Geography { get; }
    public IClientIdentifierRepository ClientIdentifiers { get; }

    public UnitOfWork(
        InsuranceDbContext context, 
        IClientRepository clientRepository, 
        IBuildingRepository buildingRepository,
        IGeographyRepository geographyRepository,
        IClientIdentifierRepository clientIdentifierRepository)
    {
        Clients = clientRepository;
        Buildings = buildingRepository;
        Geography = geographyRepository;
        ClientIdentifiers = clientIdentifierRepository;
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
