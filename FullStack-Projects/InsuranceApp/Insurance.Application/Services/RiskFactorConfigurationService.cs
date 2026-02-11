using AutoMapper;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
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
        _logger.LogInformation(
            "Creating RiskFactorConfiguration (Level={Level}, ReferenceId={ReferenceId})",
            dto.Level,
            dto.ReferenceId);

        var risk = _mapper.Map<RiskFactorConfiguration>(dto);

        risk.Id = Guid.NewGuid();
        risk.IsActive = true;

        await _unitOfWork.RiskFactorConfigurations.AddAsync(risk);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while creating RiskFactorConfiguration (Level={Level})",
                dto.Level);
            throw new InvalidOperationException("Failed to create risk factor configuration.");
        }

        _logger.LogInformation("RiskFactorConfiguration {RiskId} created successfully", risk.Id);

        return risk.Id;
    }

    public async Task UpdateAsync(Guid riskFactorId, UpdateRiskFactorConfigurationDto dto)
    {
        _logger.LogInformation("Updating RiskFactorConfiguration {RiskId}", riskFactorId);

        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);

        if (risk == null)
        {
            _logger.LogWarning("RiskFactorConfiguration {RiskId} not found", riskFactorId);
            throw new InvalidOperationException("Risk factor not found");
        }

        _mapper.Map(dto, risk);

        await _unitOfWork.RiskFactorConfigurations.UpdateAsync(risk);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while updating RiskFactorConfiguration {RiskId}", riskFactorId);
            throw new InvalidOperationException("Failed to update risk factor configuration.");
        }

        _logger.LogInformation("RiskFactorConfiguration {RiskId} updated successfully", riskFactorId);
    }

    public async Task<GetRiskFactorConfigurationByIdDto> GetByIdAsync(Guid riskFactorId)
    {
        _logger.LogInformation("Retrieving RiskFactorConfiguration {RiskId}", riskFactorId);

        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);

        if (risk == null)
        {
            _logger.LogWarning("RiskFactorConfiguration {RiskId} not found", riskFactorId);
            throw new InvalidOperationException("Risk factor not found");
        }

        _logger.LogInformation("RiskFactorConfiguration {RiskId} retrieved successfully", riskFactorId);

        return _mapper.Map<GetRiskFactorConfigurationByIdDto>(risk);
    }

    public async Task<IReadOnlyList<GetRiskFactorConfigurationsDto>> GetAllAsync()
    {
        var risks = await _unitOfWork.RiskFactorConfigurations.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} risk factor configurations", risks.Count);
        return _mapper.Map<IReadOnlyList<GetRiskFactorConfigurationsDto>>(risks);
    }
}
