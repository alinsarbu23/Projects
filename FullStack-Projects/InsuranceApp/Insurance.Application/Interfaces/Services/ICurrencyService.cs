using Insurance.Application.DTOs.Currencies;

namespace Insurance.Application.Interfaces.Services;

public interface ICurrencyService
{
    Task<Guid> CreateAsync(CreateCurrencyDto dto);

    Task UpdateAsync(Guid currencyId, UpdateCurrencyDto dto);

    Task<GetCurrencyByIdDto> GetByIdAsync(Guid currencyId);

    Task<IReadOnlyList<GetCurrenciesDto>> GetAllAsync();
}
