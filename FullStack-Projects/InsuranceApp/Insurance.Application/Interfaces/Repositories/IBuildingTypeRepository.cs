namespace Insurance.Application.Interfaces.Repositories;

public interface IBuildingTypeRepository
{
    Task<bool> ExistsAsync(Guid id);
}
