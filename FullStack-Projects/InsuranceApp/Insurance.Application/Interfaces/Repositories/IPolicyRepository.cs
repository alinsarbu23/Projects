using Insurance.Application.DTOs.Policies;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;

public interface IPolicyRepository
{
    Task AddAsync(Policy policy);

    Task UpdateAsync(Policy policy);

    Task<Policy?> GetByIdAsync(Guid id);

    Task<bool> ExistsActivePolicyWithCurrencyAsync(Guid currencyId);

    Task<bool> ExistsActiveByBuildingAsync(Guid buildingId);


    public Task<IReadOnlyList<Policy>> SearchAsync(
       Guid? clientId,
       Guid brokerId,
       PolicyStatus? status,
       DateTime? startDate,
       DateTime? endDate,
       int page,
       int pageSize);

    public Task<int> CountAsync(
     Guid? clientId,
     Guid brokerId,
     PolicyStatus? status,
     DateTime? startDate,
     DateTime? endDate);

    Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportByBrokerAsync(
        DateTime? from,
        DateTime? to);

    Task<int> ExpireActivePoliciesAsync(DateTime today);
}
