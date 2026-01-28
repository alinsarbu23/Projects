namespace Insurance.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IClientRepository Clients { get; }
    IBuildingRepository Buildings { get; }
    IGeographyRepository Geography { get; }
    IClientIdentifierRepository ClientIdentifiers { get; }
    IBuildingTypeRepository BuildingTypes { get; }
    ICityRepository Cities { get; }

    Task<int> SaveChangesAsync();
}
