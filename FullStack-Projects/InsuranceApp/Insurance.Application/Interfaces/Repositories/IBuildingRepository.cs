using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;

public interface IBuildingRepository : IRepository<Building>
{
    Task<IReadOnlyList<Building>> GetBuildingsByClientIdAsync(Guid clientId);
}
