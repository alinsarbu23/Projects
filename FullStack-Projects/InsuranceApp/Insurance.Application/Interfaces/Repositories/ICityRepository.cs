namespace Insurance.Application.Interfaces.Repositories;

public interface ICityRepository
{
    Task<bool> ExistsAsync(Guid id);
}

