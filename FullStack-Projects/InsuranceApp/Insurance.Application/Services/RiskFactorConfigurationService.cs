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
        var risk = _mapper.Map<RiskFactorConfiguration>(dto);

        risk.Id = Guid.NewGuid();
        risk.IsActive = true;

        await _unitOfWork.RiskFactorConfigurations.AddAsync(risk);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("RiskFactorConfiguration {RiskId} created", risk.Id);

        return risk.Id;
    }

    public async Task UpdateAsync(Guid riskFactorId, UpdateRiskFactorConfigurationDto dto)
    {
        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);
        if (risk == null)
        {
            throw new InvalidOperationException("Risk factor not found");
        }

        _mapper.Map(dto, risk);

        await _unitOfWork.RiskFactorConfigurations.UpdateAsync(risk);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("RiskFactorConfiguration {RiskId} updated", riskFactorId);
    }

    public async Task<GetRiskFactorConfigurationByIdDto> GetByIdAsync(Guid riskFactorId)
    {
        var risk = await _unitOfWork.RiskFactorConfigurations.GetByIdAsync(riskFactorId);
        if (risk == null)
        {
            throw new InvalidOperationException("Risk factor not found");
        }
        _logger.LogInformation("Retrieved RiskFactorConfiguration {RiskId}", riskFactorId);
        return _mapper.Map<GetRiskFactorConfigurationByIdDto>(risk);
    }

    public async Task<IReadOnlyList<GetRiskFactorConfigurationsDto>> GetAllAsync()
    {
        var risks = await _unitOfWork.RiskFactorConfigurations.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} risk factor configurations", risks.Count);
        return _mapper.Map<IReadOnlyList<GetRiskFactorConfigurationsDto>>(risks);
    }
}
