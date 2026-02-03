using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IBrokerRepository
{
    Task<Broker?> GetByIdAsync(Guid id);
    Task<Broker?> GetByCodeAsync(string brokerCode);
    Task<Broker?> GetByEmailAsync(string email);
    Task<IReadOnlyList<Broker>> GetAllAsync();
    Task<bool> IsActiveAsync(Guid brokerId);
    Task AddAsync(Broker broker);
    Task UpdateAsync(Broker broker);
}
