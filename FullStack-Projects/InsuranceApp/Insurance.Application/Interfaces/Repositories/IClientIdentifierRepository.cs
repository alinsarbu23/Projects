using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IClientIdentifierRepository
{
    Task AddAsync(ClientIdentifier identifier);
    Task<DateTime?> GetLastChangeDateAsync(Guid clientId);

}
