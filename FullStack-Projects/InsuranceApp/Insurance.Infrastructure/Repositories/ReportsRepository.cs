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

    public async Task<IReadOnlyList<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var grouped = await query
            .GroupBy(p => new
            {
                BrokerId = p.BrokerId,
                BrokerCode = p.Broker.BrokerCode,
                BrokerName = p.Broker.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new
            {
                g.Key.BrokerId,
                g.Key.BrokerCode,
                g.Key.BrokerName,
                g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .ToListAsync();

        return grouped
            .Select(x => new GetPoliciesByBrokerReportDto
            {
                BrokerId = x.BrokerId,
                BrokerCode = x.BrokerCode,
                BrokerName = x.BrokerName,
                CurrencyCode = x.CurrencyCode,
                PolicyCount = x.PolicyCount,
                TotalFinalPremium = x.TotalFinalPremium,
                TotalFinalPremiumInBaseCurrency = x.TotalFinalPremiumInBaseCurrency
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToList();
    }

    public async Task<IReadOnlyList<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var grouped = await query
            .GroupBy(p => new
            {
                CountryName = p.Building.City.County.Country.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new
            {
                g.Key.CountryName,
                g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .ToListAsync();

        return grouped
            .Select(x => new GetPoliciesByCountryReportDto
            {
                CountryName = x.CountryName,
                CurrencyCode = x.CurrencyCode,
                PolicyCount = x.PolicyCount,
                TotalFinalPremium = x.TotalFinalPremium,
                TotalFinalPremiumInBaseCurrency = x.TotalFinalPremiumInBaseCurrency
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToList();
    }

    public async Task<IReadOnlyList<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var grouped = await query
            .GroupBy(p => new
            {
                CountyName = p.Building.City.County.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new
            {
                g.Key.CountyName,
                g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .ToListAsync();

        return grouped
            .Select(x => new GetPoliciesByCountyReportDto
            {
                CountyName = x.CountyName,
                CurrencyCode = x.CurrencyCode,
                PolicyCount = x.PolicyCount,
                TotalFinalPremium = x.TotalFinalPremium,
                TotalFinalPremiumInBaseCurrency = x.TotalFinalPremiumInBaseCurrency
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToList();
    }

    public async Task<IReadOnlyList<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto)
    {
        IQueryable<Policy> query = _db.Policies.AsNoTracking();
        query = ApplyFilters(query, dto);

        var grouped = await query
            .GroupBy(p => new
            {
                CityName = p.Building.City.Name,
                CurrencyCode = p.Currency.Code,
                ExchangeRate = p.Currency.ExchangeRateToBase
            })
            .Select(g => new
            {
                g.Key.CityName,
                g.Key.CurrencyCode,
                PolicyCount = g.Count(),
                TotalFinalPremium = g.Sum(x => x.FinalPremium),
                TotalFinalPremiumInBaseCurrency = g.Sum(x => x.FinalPremium * g.Key.ExchangeRate)
            })
            .ToListAsync();

        return grouped
            .Select(x => new GetPoliciesByCityReportDto
            {
                CityName = x.CityName,
                CurrencyCode = x.CurrencyCode,
                PolicyCount = x.PolicyCount,
                TotalFinalPremium = x.TotalFinalPremium,
                TotalFinalPremiumInBaseCurrency = x.TotalFinalPremiumInBaseCurrency
            })
            .OrderByDescending(x => x.TotalFinalPremiumInBaseCurrency)
            .ToList();
    }
}
