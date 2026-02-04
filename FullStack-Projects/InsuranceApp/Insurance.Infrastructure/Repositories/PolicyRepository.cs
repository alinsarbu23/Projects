using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly InsuranceDbContext _dbContext;
    private readonly IMapper _mapper;

    public PolicyRepository(InsuranceDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(Policy policy)
    {
        var dao = _mapper.Map<DAOs.Policy>(policy);
        await _dbContext.Policies.AddAsync(dao);
    }

    public async Task<Policy?> GetByIdAsync(Guid id)
    {
        var dao = await _dbContext.Policies
            .AsNoTracking()
            .Include(p => p.Building)
            .Include(p => p.Currency)
            .FirstOrDefaultAsync(p => p.Id == id);

        return _mapper.Map<Policy?>(dao);
    }

    public async Task<IReadOnlyList<Policy>> SearchAsync(
        Guid? clientId,
        Guid? brokerId,
        PolicyStatus? status,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = _dbContext.Policies
            .AsNoTracking()
            .Include(p => p.Building)
            .AsQueryable();

        if (brokerId.HasValue)
        {
            query = query.Where(p => p.BrokerId == brokerId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value.ToString());
        }

        if (startDate.HasValue)
        {
            query = query.Where(p => p.StartDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(p => p.EndDate <= endDate.Value);
        }

        if (clientId.HasValue)
        {
            query = query.Where(p => p.Building.ClientId == clientId.Value);
        }

        var daos = await query.ToListAsync();
        return _mapper.Map<IReadOnlyList<Policy>>(daos);
    }

    public async Task UpdateAsync(Policy policy)
    {
        var dao = await _dbContext.Policies.FirstAsync(p => p.Id == policy.Id);
        _mapper.Map(policy, dao);
    }
}
