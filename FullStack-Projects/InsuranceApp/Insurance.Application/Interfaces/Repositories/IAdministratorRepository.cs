using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IAdministratorRepository
{
    Task<Administrator?> GetByIdAsync(Guid id);
    Task<Administrator?> GetByEmailAsync(string email);
}
