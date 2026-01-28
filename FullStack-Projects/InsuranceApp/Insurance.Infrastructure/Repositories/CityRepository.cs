using Insurance.Application.Interfaces.Repositories;
using Insurance.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly InsuranceDbContext _context;

    public CityRepository(InsuranceDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _context.Cities
            .AsNoTracking()
            .AnyAsync(c => c.Id == id);
    }
}
