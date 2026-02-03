using Insurance.Domain.Enums;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IFeeConfigurationRepository
{
    Task<IReadOnlyList<FeeConfiguration>> GetActiveAsync(
        FeeType? type,
        DateTime atDate);

    Task<IReadOnlyList<FeeConfiguration>> GetAllAsync();
    Task<FeeConfiguration?> GetByIdAsync(Guid id);

    Task AddAsync(FeeConfiguration fee);
    Task UpdateAsync(FeeConfiguration fee);
}
