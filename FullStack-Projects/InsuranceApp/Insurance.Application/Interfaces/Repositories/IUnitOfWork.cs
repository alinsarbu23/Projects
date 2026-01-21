namespace Insurance.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IClientRepository Clients { get; }
    IBuildingRepository Buildings { get; }
    IGeographyRepository Geography { get; }
    IClientIdentifierRepository ClientIdentifiers { get; }

    Task<int> SaveChangesAsync();
}
