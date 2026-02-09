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
        var fee = _mapper.Map<FeeConfiguration>(dto);
        fee.Id = Guid.NewGuid();
        fee.IsActive = true;

        await _unitOfWork.FeeConfigurations.AddAsync(fee);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("FeeConfiguration {FeeId} created", fee.Id);

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
        var fee = await _unitOfWork.FeeConfigurations.GetByIdAsync(feeConfigurationId);
        if (fee == null)
        {
            throw new InvalidOperationException("Fee configuration not found");
        }
        _logger.LogInformation("Retrieved FeeConfiguration {FeeId}", feeConfigurationId);
        return _mapper.Map<GetFeeConfigurationByIdDto>(fee);
    }

    public async Task UpdateAsync(Guid feeConfigurationId, UpdateFeeConfigurationDto dto)
    {
        var fee = await _unitOfWork.FeeConfigurations.GetByIdAsync(feeConfigurationId);
        if (fee == null)
        {
            throw new InvalidOperationException("Fee configuration not found");
        }

        _mapper.Map(dto, fee);

        await _unitOfWork.FeeConfigurations.UpdateAsync(fee);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("FeeConfiguration {FeeId} updated", feeConfigurationId);
    }
}
