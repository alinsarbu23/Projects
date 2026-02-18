using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly InsuranceDbContext _db;

    public ReportsRepository(InsuranceDbContext db)
    {
        _db = db;
    }

    private IQueryable<Policy> ApplyFilters(IQueryable<Policy> query, GetPoliciesReportDto dto)
    {
        if (dto.From.HasValue)
        {
            query = query.Where(p => p.StartDate >= dto.From.Value);
        }

        if (dto.To.HasValue)
        {
            query = query.Where(p => p.StartDate <= dto.To.Value);
        }

        if (dto.Status.HasValue)
        {
            query = query.Where(p => p.Status == dto.Status.Value.ToString());
        }

        if (!string.IsNullOrWhiteSpace(dto.CurrencyCode))
        {
            query = query.Where(p => p.Currency.Code == dto.CurrencyCode);
        }

        if (dto.BuildingTypeId.HasValue)
        {
            query = query.Where(p => p.Building.BuildingTypeId == dto.BuildingTypeId.Value);
        }

        return query;
    }

    public async Task<IReadOnlyList<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        return await query
            .GroupBy(p => new
            {
                BrokerId = p.BrokerId,
                BrokerCode = p.Broker.BrokerCode,
                BrokerName = p.Broker.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new GetPoliciesByBrokerReportDto
            {
                BrokerId = g.Key.BrokerId,
                BrokerCode = g.Key.BrokerCode,
                BrokerName = g.Key.BrokerName,
                CurrencyCode = g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency =
                    g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        return await query
            .GroupBy(p => new
            {
                CountryName = p.Building.City.County.Country.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new GetPoliciesByCountryReportDto
            {
                CountryName = g.Key.CountryName,
                CurrencyCode = g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency =
                    g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        return await query
            .GroupBy(p => new
            {
                CountyName = p.Building.City.County.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new GetPoliciesByCountyReportDto
            {
                CountyName = g.Key.CountyName,
                CurrencyCode = g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency =
                    g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        return await query
            .GroupBy(p => new
            {
                CityName = p.Building.City.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new GetPoliciesByCityReportDto
            {
                CityName = g.Key.CityName,
                CurrencyCode = g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency =
                    g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToListAsync();
    }
}
