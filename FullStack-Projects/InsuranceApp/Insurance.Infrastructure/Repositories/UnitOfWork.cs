using Insurance.Application.Interfaces.Repositories;

namespace Insurance.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InsuranceDbContext _context;
    public IClientRepository Clients { get; }
    public IBuildingRepository Buildings { get; }
    public IGeographyRepository Geography { get; }

    public UnitOfWork(
        InsuranceDbContext context, 
        IClientRepository clientRepository, 
        IBuildingRepository buildingRepository,
        IGeographyRepository geographyRepository)
    {
        Clients = clientRepository;
        Buildings = buildingRepository;
        Geography = geographyRepository;
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
