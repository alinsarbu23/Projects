using AutoMapper;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class PolicyService : IPolicyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPolicyCalculationService _calculationService;
    private readonly IMapper _mapper;
    private readonly ILogger<PolicyService> _logger;

    public PolicyService(
        IUnitOfWork unitOfWork,
        IPolicyCalculationService calculationService,
        IMapper mapper,
        ILogger<PolicyService> logger)
    {
        _unitOfWork = unitOfWork;
        _calculationService = calculationService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateDraftAsync(CreatePolicyDto dto, Guid brokerId)
    {
        if (!await _unitOfWork.Brokers.IsActiveAsync(brokerId))
        {
            _logger.LogWarning("Inactive broker {BrokerId} tried to create policy", brokerId);
            throw new InvalidOperationException("Broker is not active");
        }

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if(broker == null)
        {
            _logger.LogWarning("Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        var building = await _unitOfWork.Buildings.GetByIdAsync(dto.BuildingId);
        if(building == null)
        {
            _logger.LogWarning("Building {BuildingId} not found for policy creation", dto.BuildingId);
            throw new InvalidOperationException("Building not found");
        }

        var finalPremium = await _calculationService.CalculateFinalPremiumAsync(dto.BasePremium, brokerId, building);

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            PolicyNumber = GeneratePolicyNumber(),

            BuildingId = building.Id,
            Building = building,

            BrokerId = broker.Id,
            Broker = broker,

            CurrencyId = dto.CurrencyId,

            StartDate = dto.StartDate,
            EndDate = dto.EndDate,

            BasePremium = dto.BasePremium,
            FinalPremium = finalPremium,

            Status = PolicyStatus.Draft
        };

        await _unitOfWork.Policies.AddAsync(policy);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Policy {PolicyNumber} created", policy.PolicyNumber);

        return policy.Id;
    }

    public async Task ActivateAsync(Guid policyId)
    {
        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);
        if(policy == null)
        {
            _logger.LogWarning("Policy {PolicyId} not found for activation", policyId);
            throw new InvalidOperationException("Policy not found");
        }

        if (policy.Status != PolicyStatus.Draft)
        {
            _logger.LogWarning("Attempt to activate non-draft policy {PolicyId}", policyId);
            throw new InvalidOperationException("Only draft policies can be activated");
        }

        if (policy.StartDate < DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Attempt to activate policy {PolicyId} with past start date", policyId);
            throw new InvalidOperationException("Policy start date cannot be in the past");
        }

        policy.Status = PolicyStatus.Active;
        policy.ActivatedAt = DateTime.UtcNow;

        await _unitOfWork.Policies.UpdateAsync(policy);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Policy {PolicyId} activated", policyId);
    }

    public async Task CancelAsync(Guid policyId, DeletePolicyDto dto)
    {
        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);
        if(policy == null)
        {
            _logger.LogWarning("Policy {PolicyId} not found", policyId);
            throw new InvalidOperationException("Policy not found");
        }

        if (policy.Status != PolicyStatus.Active)
        {
            _logger.LogWarning("Attempt to cancel non-active policy {PolicyId}", policyId);
            throw new InvalidOperationException("Only active policies can be cancelled");
        }

        policy.Status = PolicyStatus.Cancelled;
        policy.CancelledAt = DateTime.UtcNow;
        policy.CancellationReason = dto.Reason;

        await _unitOfWork.Policies.UpdateAsync(policy);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Policy {PolicyId} cancelled", policyId);
    }

    public async Task<GetPolicyByIdDto> GetByIdAsync(Guid policyId)
    {
        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);
        if(policy == null)
        {
            _logger.LogWarning("Policy {PolicyId} not found", policyId);
            throw new InvalidOperationException("Policy not found");
        }

        return _mapper.Map<GetPolicyByIdDto>(policy);
    }

    public async Task<IReadOnlyList<GetPoliciesDto>> SearchAsync(
        Guid? clientId,
        Guid? brokerId,
        string? status,
        DateTime? startDate,
        DateTime? endDate)
    {
        PolicyStatus? parsedStatus = null;

        if (!string.IsNullOrWhiteSpace(status))
        {
            parsedStatus = Enum.Parse<PolicyStatus>(status, true);
        }

        var policies = await _unitOfWork.Policies.SearchAsync(
            clientId,
            brokerId,
            parsedStatus,
            startDate,
            endDate);

        return _mapper.Map<IReadOnlyList<GetPoliciesDto>>(policies);
    }

    private static string GeneratePolicyNumber()
    {
        return $"POL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N[..4]}";
    }
}
