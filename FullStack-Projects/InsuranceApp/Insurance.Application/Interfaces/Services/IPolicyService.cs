using Insurance.Application.DTOs.Policies;

namespace Insurance.Application.Interfaces.Services;

public interface IPolicyService
{
    Task<Guid> CreateDraftAsync(CreatePolicyDto dto, Guid brokerId);

    Task ActivateAsync(Guid policyId);

    Task CancelAsync(Guid policyId, DeletePolicyDto dto);

    Task<GetPolicyByIdDto> GetByIdAsync(Guid policyId);

    Task<IReadOnlyList<GetPoliciesDto>> SearchAsync(
        Guid? clientId,
        Guid? brokerId,
        string? status,
        DateTime? startDate,
        DateTime? endDate);
}
