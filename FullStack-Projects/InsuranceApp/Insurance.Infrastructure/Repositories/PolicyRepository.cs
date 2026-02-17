using AutoMapper;
using Insurance.Application.DTOs.Policies;
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

    public async Task<int> CountAsync(
        Guid? clientId,
        Guid brokerId,
        PolicyStatus? status,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = _dbContext.Policies
            .AsNoTracking()
            .Include(p => p.Building)
            .AsQueryable();
        query = query.Where(p => p.BrokerId == brokerId);

        if (clientId.HasValue)
            query = query.Where(p => p.Building.ClientId == clientId.Value);

        if (status.HasValue)
        {
            if (status == PolicyStatus.Expired)
            {
                query = query.Where(p => p.EndDate < DateTime.UtcNow);
            }
            else
            {
                query = query.Where(p => p.Status == status.Value.ToString());
            }
        }

        if (startDate.HasValue)
            query = query.Where(p => p.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.EndDate <= endDate.Value);

        return await query.CountAsync();
    }

    public async Task<IReadOnlyList<Policy>> SearchAsync(
        Guid? clientId,
        Guid brokerId,
        PolicyStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize)
    {
        var query = _dbContext.Policies
            .AsNoTracking()
            .Include(p => p.Building)
            .AsQueryable();

        query = query.Where(p => p.BrokerId == brokerId);

        if (clientId.HasValue)
            query = query.Where(p => p.Building.ClientId == clientId.Value);

        if (status.HasValue)
        {
            if (status == PolicyStatus.Expired)
            {
                query = query.Where(p => p.EndDate < DateTime.UtcNow);
            }
            else
            {
                query = query.Where(p => p.Status == status.Value.ToString());
            }
        }

        if (startDate.HasValue)
            query = query.Where(p => p.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.EndDate <= endDate.Value);

        var daos = await query
            .OrderByDescending(p => p.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<Policy>>(daos);
    }

    public async Task UpdateAsync(Policy policy)
    {
        var dao = await _dbContext.Policies.FirstAsync(p => p.Id == policy.Id);

        dao.CurrencyId = policy.CurrencyId;
        dao.StartDate = policy.StartDate;
        dao.EndDate = policy.EndDate;
        dao.BasePremium = policy.BasePremium;
        dao.FinalPremium = policy.FinalPremium;
        dao.Status = policy.Status.ToString();
        dao.ActivatedAt = policy.ActivatedAt;
        dao.CancelledAt = policy.CancelledAt;
        dao.CancellationReason = policy.CancellationReason;
    }

    public async Task<IReadOnlyList<PolicyReportDto>> GetPolicyReportByBrokerAsync(DateTime? from, DateTime? to)
    {
        var query = _dbContext.Policies
            .AsNoTracking()
            .Include(p => p.Broker)
            .AsQueryable();

        query = query.Where(p => p.Status == PolicyStatus.Active.ToString());

        if (from.HasValue)
            query = query.Where(p => p.StartDate >= from.Value);

        if (to.HasValue)
            query = query.Where(p => p.EndDate <= to.Value);

        return await query
            .GroupBy(p => new
            {
                p.BrokerId,
                p.Broker.BrokerCode,
                p.Broker.Name
            })
            .Select(g => new PolicyReportDto
            {
                BrokerId = g.Key.BrokerId,
                BrokerCode = g.Key.BrokerCode,
                BrokerName = g.Key.Name,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium)
            })
            .OrderByDescending(x => x.TotalFinalPremium)
            .ToListAsync();
    }

    public async Task<bool> ExistsActivePolicyWithCurrencyAsync(Guid currencyId)
    {
        return await _dbContext.Policies
            .AsNoTracking()
            .AnyAsync(p =>
                p.CurrencyId == currencyId &&
                p.Status == PolicyStatus.Active.ToString());
    }
}
