using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;
public interface IBuildingRepository : IRepository<Building>
{
    Task<IReadOnlyList<Building>> GetBuildingsByClientIdAsync(Guid clientId);
}
