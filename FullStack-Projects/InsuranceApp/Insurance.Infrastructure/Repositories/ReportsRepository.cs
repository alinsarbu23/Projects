using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Enums;
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
            query = query.Where(p => p.EndDate <= dto.To.Value);
        }

        if (dto.Status.HasValue)
        {
            if (dto.Status == PolicyStatus.Expired)
            {
                query = query.Where(p => p.EndDate < DateTime.UtcNow);
            }
            else
            {
                query = query.Where(p => p.Status == dto.Status.Value.ToString());
            }
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

    public async Task<(IReadOnlyList<GetPoliciesByBrokerReportDto> Items, int TotalCount)> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var groupedQuery = query
            .GroupBy(p => new
            {
                p.BrokerId,
                p.Broker.BrokerCode,
                p.Broker.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new GetPoliciesByBrokerReportDto
            {
                BrokerId = g.Key.BrokerId,
                BrokerCode = g.Key.BrokerCode,
                BrokerName = g.Key.Name,
                CurrencyCode = g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            });

        var totalCount = await groupedQuery.CountAsync();

        var items = await groupedQuery
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<GetPoliciesByCountryReportDto> Items, int TotalCount)> GetPoliciesByCountryAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var groupedQuery = query
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
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            });

        var totalCount = await groupedQuery.CountAsync();

        var items = await groupedQuery
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<GetPoliciesByCountyReportDto> Items, int TotalCount)> GetPoliciesByCountyAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var groupedQuery = query
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
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            });

        var totalCount = await groupedQuery.CountAsync();

        var items = await groupedQuery
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<GetPoliciesByCityReportDto> Items, int TotalCount)> GetPoliciesByCityAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var groupedQuery = query
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
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            });

        var totalCount = await groupedQuery.CountAsync();

        var items = await groupedQuery
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
