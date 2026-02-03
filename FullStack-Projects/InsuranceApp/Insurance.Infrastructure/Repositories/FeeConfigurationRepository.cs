using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class FeeConfigurationRepository : IFeeConfigurationRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public FeeConfigurationRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<FeeConfiguration>> GetActiveAsync(FeeType? type, DateTime atDate)
    {
        var query = _context.FeeConfigurations.AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.EffectiveFrom <= atDate &&
                (x.EffectiveTo == null || x.EffectiveTo >= atDate));

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type.ToString());
        }

        return _mapper.Map<IReadOnlyList<FeeConfiguration>>(await query.ToListAsync());
    }

    public async Task<IReadOnlyList<FeeConfiguration>> GetAllAsync()
    {
        return _mapper.Map<IReadOnlyList<FeeConfiguration>>(await _context.FeeConfigurations.AsNoTracking().ToListAsync());
    }

    public async Task<FeeConfiguration?> GetByIdAsync(Guid id)
    {
        var dao = await _context.FeeConfigurations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<FeeConfiguration?>(dao);
    }

    public async Task AddAsync(FeeConfiguration fee)
    {
        await _context.FeeConfigurations.AddAsync(_mapper.Map<DAOs.FeeConfiguration>(fee));
    }

    public async Task UpdateAsync(FeeConfiguration fee)
    {
        var dao = await _context.FeeConfigurations.FirstAsync(x => x.Id == fee.Id);
        _mapper.Map(fee, dao);
    }
}
