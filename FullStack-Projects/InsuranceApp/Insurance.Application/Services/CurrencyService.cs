using AutoMapper;
using Insurance.Application.DTOs.Currencies;
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
        var currency = _mapper.Map<Currency>(dto);

        currency.Id = Guid.NewGuid();
        currency.IsActive = true;

        await _unitOfWork.Currencies.AddAsync(currency);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Currency {Code} created", currency.Code);

        return currency.Id;
    }

    public async Task UpdateAsync(Guid currencyId, UpdateCurrencyDto dto)
    {
        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);
        if (currency == null)
        {
            throw new InvalidOperationException("Currency not found");
        }

        if (dto.IsActive == 0 && currency.IsActive)
        {
            var isUsedInActivePolicies = await _unitOfWork.Policies.ExistsActivePolicyWithCurrencyAsync(currencyId);

            if (isUsedInActivePolicies)
            {
                throw new InvalidOperationException("Cannot deactivate currency used by active policies");
            }
        }

        _mapper.Map(dto, currency);

        await _unitOfWork.Currencies.UpdateAsync(currency);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task<GetCurrencyByIdDto> GetByIdAsync(Guid currencyId)
    {
        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);
        if (currency == null)
        {
            throw new InvalidOperationException("Currency not found");
        }

        _logger.LogInformation("Currency {CurrencyId} retrieved", currencyId);
        return _mapper.Map<GetCurrencyByIdDto>(currency);
    }

    public async Task<IReadOnlyList<GetCurrenciesDto>> GetAllAsync()
    {
        var currencies = await _unitOfWork.Currencies.GetAllAsync();
        _logger.LogInformation("All currencies retrieved");
        return _mapper.Map<IReadOnlyList<GetCurrenciesDto>>(currencies);
    }
}
