using AutoMapper;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Exceptions;
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

    public async Task ActivateAsync(Guid policyId)
    {
        _logger.LogInformation($"Activating policy {policyId}");

        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);

        if (policy == null)
        {
            _logger.LogWarning($"Policy {policyId} not found");
            throw new NotFoundException("Policy not found.");
        }

        if (policy.Status != PolicyStatus.Draft)
        {
            _logger.LogWarning(
                $"Activation blocked. Policy {policyId} is not in Draft state");

            throw new InvalidOperationException(
                "Only draft policies can be activated.");
        }

        if (policy.EndDate < DateTime.UtcNow.Date)
        {
            _logger.LogWarning(
                $"Activation blocked. Policy {policyId} already ended");

            throw new InvalidOperationException(
                "Cannot activate a policy that already ended.");
        }

        var existsActivePolicy = await _unitOfWork.Policies
            .ExistsActiveByBuildingAsync(policy.BuildingId);

        if (existsActivePolicy)
        {
            _logger.LogWarning(
                $"Activation blocked. Building {policy.BuildingId} already has an active policy");

            throw new InvalidOperationException(
                "There is already an active policy for this building.");
        }

        policy.Status = PolicyStatus.Active;
        policy.ActivatedAt = DateTime.UtcNow;

        await _unitOfWork.Policies.UpdateAsync(policy);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            _logger.LogError($"Failed to activate policy {policyId}");
            throw new InvalidOperationException("Failed to activate policy.");
        }

        _logger.LogInformation($"Policy {policyId} activated successfully");
    }

    public async Task CancelAsync(Guid policyId, DeletePolicyDto dto)
    {
        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);

        if (policy == null)
        {
            throw new NotFoundException("Policy not found");
        }

        if (policy.Status == PolicyStatus.Cancelled)
        {
            throw new InvalidOperationException("Policy is already cancelled.");
        }

        if (policy.Status != PolicyStatus.Active)
        {
            throw new InvalidOperationException("Only active policies can be cancelled.");
        }

        policy.Status = PolicyStatus.Cancelled;
        policy.CancelledAt = DateTime.UtcNow;
        policy.CancellationReason = dto.Reason;

        await _unitOfWork.Policies.UpdateAsync(policy);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation($"Policy {policyId} cancelled");
    }

    public async Task<GetPolicyByIdDto> GetByIdAsync(Guid policyId)
    {
        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);
        if (policy == null)
        {
            _logger.LogWarning($"Policy {policyId} not found");
            throw new NotFoundException("Policy not found");
        }

        return _mapper.Map<GetPolicyByIdDto>(policy);
    }

    private static string GeneratePolicyNumber()
    {
        var randomPart = Guid.NewGuid()
            .ToString("N")
            .Substring(0, 6)
            .ToUpper();

        return $"POL-{DateTime.UtcNow:yyyyMMddHHmmss}-{randomPart}";
    }

    public async Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportByBrokerAsync(DateTime? from, DateTime? to)
    {
        var result = await _unitOfWork.Policies.GetPolicyReportByBrokerAsync(from, to);

        _logger.LogInformation(
            $"Policy report generated (grouped by broker). From={from}, To={to}");

        return result;
    }

    public async Task<Guid> CreateDraftAsync(CreatePolicyDto dto, string brokerEmail)
    {
        _logger.LogInformation($"Creating draft policy for building {dto.BuildingId}");

        var broker = await _unitOfWork.Brokers.GetByEmailAsync(brokerEmail);

        if (broker == null)
        {
            _logger.LogWarning($"Broker with email {brokerEmail} not found");
            throw new NotFoundException("Broker not found.");
        }

        if (broker.Status != BrokerStatus.Active)
        {
            _logger.LogWarning($"Broker {broker.Id} is not active");
            throw new InvalidOperationException("Broker is not active.");
        }

        var building = await _unitOfWork.Buildings.GetByIdAsync(dto.BuildingId);

        if (building == null)
        {
            _logger.LogWarning($"Building {dto.BuildingId} not found");
            throw new NotFoundException("Building not found.");
        }

        var existsActivePolicy = await _unitOfWork.Policies
            .ExistsActiveByBuildingAsync(dto.BuildingId);

        if (existsActivePolicy)
        {
            _logger.LogWarning(
                $"Attempt to create policy for building {dto.BuildingId} which already has an active policy");

            throw new InvalidOperationException(
                "There is already an active policy for this building.");
        }

        var finalPremium = await _calculationService.CalculateFinalPremiumAsync(
            dto.BasePremium,
            broker.Id,
            dto.CurrencyId,
            building);

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            PolicyNumber = GeneratePolicyNumber(),
            BrokerId = broker.Id,
            BuildingId = building.Id,
            CurrencyId = dto.CurrencyId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            BasePremium = dto.BasePremium,
            FinalPremium = finalPremium,
            Status = PolicyStatus.Draft
        };

        await _unitOfWork.Policies.AddAsync(policy);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            _logger.LogError($"Failed to create draft policy for building {dto.BuildingId}");
            throw new InvalidOperationException("Failed to create policy.");
        }

        _logger.LogInformation($"Draft policy {policy.Id} created successfully");

        return policy.Id;
    }

    public async Task UpdatePolicyAsync(Guid policyId, UpdatePolicyDto dto)
    {
        _logger.LogInformation($"Updating policy {policyId}");

        var policy = await _unitOfWork.Policies.GetByIdAsync(policyId);

        if (policy == null)
        {
            _logger.LogWarning($"Policy {policyId} not found");
            throw new NotFoundException("Policy not found.");
        }

        if (policy.Status != PolicyStatus.Draft)
        {
            _logger.LogWarning($"Update blocked. Policy {policyId} is not Draft");
            throw new InvalidOperationException("Only draft policies can be updated.");
        }

        var currency = await _unitOfWork.Currencies.GetByIdAsync(dto.CurrencyId);

        if (currency == null || !currency.IsActive)
        {
            _logger.LogWarning($"Invalid currency {dto.CurrencyId}");
            throw new InvalidOperationException("Invalid or inactive currency.");
        }

        _mapper.Map(dto, policy);

        policy.FinalPremium = await _calculationService.CalculateFinalPremiumAsync(
            dto.BasePremium,
            policy.BrokerId,
            dto.CurrencyId,
            policy.Building
        );

        await _unitOfWork.Policies.UpdateAsync(policy);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Failed to update policy {policyId}");
            throw new InvalidOperationException("Failed to update policy.");
        }

        _logger.LogInformation($"Policy {policyId} updated successfully");
    }

    public async Task<PagedPoliciesDto> SearchAsync(
        Guid? clientId,
        string brokerEmail,
        string? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize)
    {
        var broker = await _unitOfWork.Brokers.GetByEmailAsync(brokerEmail)
            ?? throw new NotFoundException("Broker not found");

        PolicyStatus? parsedStatus = null;

        if (!string.IsNullOrWhiteSpace(status))
        {
            parsedStatus = Enum.Parse<PolicyStatus>(status, true);
        }

        var totalCount = await _unitOfWork.Policies.CountAsync(
            clientId,
            broker.Id,
            parsedStatus,
            startDate,
            endDate);

        var policies = await _unitOfWork.Policies.SearchAsync(
            clientId,
            broker.Id,
            parsedStatus,
            startDate,
            endDate,
            page,
            pageSize);

        return new PagedPoliciesDto
        {
            Items = _mapper.Map<IReadOnlyList<GetPoliciesDto>>(policies),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
