using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task AddAsync(Client client);

    Task<Client?> GetByIdAsync(Guid clientId);

    Task<IReadOnlyList<Client>> SearchAsync(string? name, string? identifier);

    Task UpdateAsync(Client client);

    Task<bool> ExistsByCnpAsync(string cnp);
    Task<bool> ExistsByCuiAsync(string cui);
}
