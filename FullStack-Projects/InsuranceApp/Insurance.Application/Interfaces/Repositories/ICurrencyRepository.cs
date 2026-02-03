using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task<IReadOnlyList<Currency>> GetActiveAsync();
    Task<Currency?> GetByCodeAsync(string code);

    Task<IReadOnlyList<Currency>> GetAllAsync();
    Task<Currency?> GetByIdAsync(Guid id);

    Task AddAsync(Currency currency);
    Task UpdateAsync(Currency currency);
}
