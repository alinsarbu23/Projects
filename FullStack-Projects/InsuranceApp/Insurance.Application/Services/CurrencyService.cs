using AutoMapper;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CurrencyService> _logger;

    public CurrencyService(
        IUnitOfWork uow,
        IMapper mapper,
        ILogger<CurrencyService> logger)
    {
        _unitOfWork = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateCurrencyDto dto)
    {
        dto.Code = dto.Code.ToUpper();

        _logger.LogInformation($"Creating currency {dto.Code}");

        var exists = await _unitOfWork.Currencies
            .ExistsByCodeAsync(dto.Code);

        if (exists)
        {
            _logger.LogWarning($"Currency code {dto.Code} already exists");
            throw new InvalidOperationException("Currency code must be unique.");
        }

        var currency = _mapper.Map<Currency>(dto);
        currency.Id = Guid.NewGuid();
        currency.IsActive = true;

        await _unitOfWork.Currencies.AddAsync(currency);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            _logger.LogError($"Critical error while creating currency {dto.Code}");
            throw new InvalidOperationException("Failed to create currency.");
        }

        return currency.Id;
    }

    public async Task UpdateAsync(Guid currencyId, UpdateCurrencyDto dto)
    {
        _logger.LogInformation($"Updating currency {currencyId}");

        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);

        if (currency == null)
        {
            _logger.LogWarning($"Currency {currencyId} not found");
            throw new NotFoundException("Currency not found");
        }

        if (dto.IsActive == 1 && currency.IsActive)
        {
            throw new InvalidOperationException("Currency is already active.");
        }

        if (dto.IsActive == 0 && !currency.IsActive)
        {
            throw new InvalidOperationException("Currency is already inactive.");
        }

        if (dto.IsActive == 0 && currency.IsActive)
        {
            var isUsedInActivePolicies = await _unitOfWork.Policies.ExistsActivePolicyWithCurrencyAsync(currencyId);

            if (isUsedInActivePolicies)
            {
                _logger.LogWarning($"Attempt to deactivate currency {currencyId} used by active policies");
                throw new InvalidOperationException("Cannot deactivate currency used by active policies");
            }
        }

        _mapper.Map(dto, currency);

        await _unitOfWork.Currencies.UpdateAsync(currency);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while updating currency {currencyId}");
            throw new InvalidOperationException("Failed to update currency.");
        }

        _logger.LogInformation($"Currency {currencyId} updated successfully");
    }

    public async Task<GetCurrencyByIdDto> GetByIdAsync(Guid currencyId)
    {
        _logger.LogInformation($"Retrieving currency {currencyId}");

        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);

        if (currency == null)
        {
            _logger.LogWarning($"Currency {currencyId} not found");
            throw new NotFoundException("Currency not found");
        }

        _logger.LogInformation($"Currency {currencyId} retrieved successfully");

        return _mapper.Map<GetCurrencyByIdDto>(currency);
    }

    public async Task<IReadOnlyList<GetCurrenciesDto>> GetAllAsync()
    {
        _logger.LogInformation($"Retrieving all currencies");

        var currencies = await _unitOfWork.Currencies.GetAllAsync();

        _logger.LogInformation($"Retrieved {currencies.Count} currencies");

        return _mapper.Map<IReadOnlyList<GetCurrenciesDto>>(currencies);
    }
}
