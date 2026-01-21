using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IClientIdentifierRepository
{
    Task AddAsync(ClientIdentifier identifier);

    Task<int> CountByClientIdAsync(Guid clientId);
    Task<DateTime?> GetLastChangeDateAsync(Guid clientId);

}
