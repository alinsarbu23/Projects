using Insurance.Application.DTOs.Policies;

namespace Insurance.Application.Interfaces.Services;

public interface IPolicyService
{
    Task<Guid> CreateDraftAsync(CreatePolicyDto dto, string brokerEmail);

    Task ActivateAsync(Guid policyId);

    Task CancelAsync(Guid policyId, DeletePolicyDto dto);

    Task<GetPolicyByIdDto> GetByIdAsync(Guid policyId);

    Task<PagedPoliciesDto> SearchAsync(
        Guid? clientId,
        string brokerEmail,
        string? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize);

    Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportByBrokerAsync(DateTime? from, DateTime? to);
}
