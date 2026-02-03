using Insurance.Domain.Enums;
using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IPolicyRepository
{
    Task<Policy?> GetByIdAsync(Guid id);

    Task AddAsync(Policy policy);
    Task UpdateAsync(Policy policy);

    Task<IReadOnlyList<Policy>> SearchAsync(
        Guid? clientId,
        Guid? brokerId,
        PolicyStatus? status,
        DateTime? startDate,
        DateTime? endDate);
}
