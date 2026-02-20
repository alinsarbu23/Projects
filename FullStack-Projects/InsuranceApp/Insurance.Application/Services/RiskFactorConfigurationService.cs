using AutoMapper;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class RiskFactorConfigurationService : IRiskFactorConfigurationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RiskFactorConfigurationService> _logger;

    public RiskFactorConfigurationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RiskFactorConfigurationService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateRiskFactorConfigurationDto dto)
    {
        _logger.LogInformation($"Creating new RiskFactorConfiguration with Level {dto.Level} and ReferenceId {dto.ReferenceId}");

        if (!Enum.IsDefined(typeof(RiskLevel), dto.Level))
        {
            _logger.LogError($"Invalid risk level {dto.Level} provided");
            throw new ArgumentException("Invalid risk level.");
        }

        var risk = _mapper.Map<RiskFactorConfiguration>(dto);

        risk.Id = Guid.NewGuid();
        risk.IsActive = true;
        risk.Level = (RiskLevel)dto.Level;

        await _unitOfWork.RiskFactorConfigurations.AddAsync(risk);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while creating RiskFactorConfiguration with ReferenceId {dto.ReferenceId}");
            throw new InvalidOperationException("Failed to create risk factor configuration.");
        }

        _logger.LogInformation($"RiskFactorConfiguration with Id {risk.Id} created successfully");

        return risk.Id;
    }

    public async Task UpdateAsync(Guid riskFactorId, UpdateRiskFactorConfigurationDto dto)
    {
        _logger.LogInformation($"Updating RiskFactorConfiguration {riskFactorId}");

        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);

        if (risk == null)
        {
            _logger.LogWarning($"RiskFactorConfiguration {riskFactorId} not found");
            throw new NotFoundException("Risk factor not found");
        }

        if (dto.IsActive == 1 && risk.IsActive)
        {
            throw new InvalidOperationException("Risk factor is already active.");
        }

        if (dto.IsActive == 0 && !risk.IsActive)
        {
            throw new InvalidOperationException("Risk factor is already inactive.");
        }

        _mapper.Map(dto, risk);

        await _unitOfWork.RiskFactorConfigurations.UpdateAsync(risk);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while updating RiskFactorConfiguration {riskFactorId}");
            throw new InvalidOperationException("Failed to update risk factor configuration.");
        }

        _logger.LogInformation($"RiskFactorConfiguration {riskFactorId} updated successfully");
    }

    public async Task<GetRiskFactorConfigurationByIdDto> GetByIdAsync(Guid riskFactorId)
    {
        _logger.LogInformation($"Retrieving RiskFactorConfiguration {riskFactorId}");

        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);

        if (risk == null)
        {
            _logger.LogWarning($"RiskFactorConfiguration {riskFactorId} not found");
            throw new NotFoundException("Risk factor not found");
        }

        _logger.LogInformation($"RiskFactorConfiguration {riskFactorId} retrieved successfully");

        return _mapper.Map<GetRiskFactorConfigurationByIdDto>(risk);
    }

    public async Task<IReadOnlyList<GetRiskFactorConfigurationsDto>> GetAllAsync()
    {
        var risks = await _unitOfWork.RiskFactorConfigurations.GetAllAsync();
        _logger.LogInformation($"Retrieved {risks.Count} risk factor configurations");
        return _mapper.Map<IReadOnlyList<GetRiskFactorConfigurationsDto>>(risks);
    }
}
