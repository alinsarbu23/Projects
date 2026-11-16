using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public CurrencyRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<Currency>> GetActiveAsync()
    {
        var daos = await _context.Currencies.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<Currency>>(daos);
    }

    public async Task<Currency?> GetByCodeAsync(string code)
    {
        var dao = await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code && x.IsActive);

        return _mapper.Map<Currency?>(dao);
    }

    public async Task<IReadOnlyList<Currency>> GetAllAsync()
    {
        var daos = await _context.Currencies.AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<Currency>>(daos);
    }

    public async Task<Currency?> GetByIdAsync(Guid id)
    {
        var dao = await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<Currency?>(dao);
    }

    public async Task AddAsync(Currency currency)
    {
        await _context.Currencies.AddAsync(_mapper.Map<DAOs.Currency>(currency));
    }

    public async Task UpdateAsync(Currency currency)
    {
        var dao = await _context.Currencies.FirstAsync(x => x.Id == currency.Id);

        _mapper.Map(currency, dao);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Currencies.AnyAsync(x => x.Code.ToLower() == code.ToLower());
    }
}
