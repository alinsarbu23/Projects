using AutoMapper;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class FeeConfigurationService : IFeeConfigurationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FeeConfigurationService> _logger;

    public FeeConfigurationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<FeeConfigurationService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateFeeConfigurationDto dto)
    {
        _logger.LogInformation("Creating fee configuration {Name}", dto.Name);

        var fee = _mapper.Map<FeeConfiguration>(dto);
        fee.Id = Guid.NewGuid();
        fee.IsActive = true;

        await _unitOfWork.FeeConfigurations.AddAsync(fee);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while creating FeeConfiguration {Name}", dto.Name);
            throw new InvalidOperationException("Failed to create fee configuration.");
        }

        _logger.LogInformation("FeeConfiguration {FeeId} created successfully", fee.Id);

        return fee.Id;
    }

    public async Task<IReadOnlyList<GetFeeConfigurationsDto>> GetAllAsync()
    {
        var fees = await _unitOfWork.FeeConfigurations.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} fee configurations", fees.Count);
        return _mapper.Map<IReadOnlyList<GetFeeConfigurationsDto>>(fees);
    }

    public async Task<GetFeeConfigurationByIdDto> GetByIdAsync(Guid feeConfigurationId)
    {
        _logger.LogInformation("Retrieving FeeConfiguration {FeeId}", feeConfigurationId);

        var fee = await _unitOfWork.FeeConfigurations.GetByIdAsync(feeConfigurationId);

        if (fee == null)
        {
            _logger.LogWarning("FeeConfiguration {FeeId} not found", feeConfigurationId);
            throw new InvalidOperationException("Fee configuration not found");
        }

        _logger.LogInformation("FeeConfiguration {FeeId} retrieved successfully", feeConfigurationId);

        return _mapper.Map<GetFeeConfigurationByIdDto>(fee);
    }

    public async Task UpdateAsync(Guid feeConfigurationId, UpdateFeeConfigurationDto dto)
    {
        _logger.LogInformation("Updating FeeConfiguration {FeeId}", feeConfigurationId);

        var fee = await _unitOfWork.FeeConfigurations.GetByIdAsync(feeConfigurationId);

        if (fee == null)
        {
            _logger.LogWarning("FeeConfiguration {FeeId} not found", feeConfigurationId);
            throw new InvalidOperationException("Fee configuration not found");
        }

        _mapper.Map(dto, fee);

        await _unitOfWork.FeeConfigurations.UpdateAsync(fee);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while updating FeeConfiguration {FeeId}", feeConfigurationId);
            throw new InvalidOperationException("Failed to update fee configuration.");
        }

        _logger.LogInformation("FeeConfiguration {FeeId} updated successfully", feeConfigurationId);
    }
}
