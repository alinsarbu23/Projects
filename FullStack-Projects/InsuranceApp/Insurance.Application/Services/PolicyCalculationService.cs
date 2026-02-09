using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class PolicyCalculationService : IPolicyCalculationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PolicyCalculationService> _logger;

    public PolicyCalculationService(IUnitOfWork unitOfWork, ILogger<PolicyCalculationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<decimal> CalculateFinalPremiumAsync(
        decimal basePremium,
        Guid brokerId,
        Guid currencyId,
        Building building)
    {
        decimal totalPercentage = 0m;

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId)
            ?? throw new InvalidOperationException("Broker not found");

        if (broker.CommissionPercentage > 0)
        {
            totalPercentage += broker.CommissionPercentage;
        }

        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);
        if (currency == null || !currency.IsActive)
        {
            throw new InvalidOperationException("Invalid or inactive currency");
        }

        var premiumInBaseCurrency = basePremium * currency.ExchangeRateToBase;

        var fees = await _unitOfWork.FeeConfigurations
            .GetActiveAsync(null, DateTime.UtcNow);

        foreach (var fee in fees)
        {
            if (fee.Type == FeeType.RiskAdjustment)
            {
                if (string.IsNullOrWhiteSpace(fee.AppliesToRisk))
                    continue;

                if (!Enum.TryParse<BuildingRiskType>(
                        fee.AppliesToRisk,
                        ignoreCase: true,
                        out var riskType))
                    continue;

                var applies = riskType switch
                {
                    BuildingRiskType.Earthquake => building.EarthquakeRisk,
                    BuildingRiskType.Flood => building.FloodRisk,
                    _ => false
                };

                if (applies)
                {
                    totalPercentage += fee.Percentage;
                }
            }
            else
            {
                totalPercentage += fee.Percentage;
            }
        }

        var risks = await _unitOfWork.RiskFactorConfigurations
            .GetActiveForBuildingAsync(
                building.City.County.CountryId,
                building.City.CountyId,
                building.CityId,
                building.BuildingTypeId);

        foreach (var risk in risks)
        {
            totalPercentage += risk.AdjustmentPercentage;
        }

        var finalPremiumInBase = premiumInBaseCurrency * (1 + totalPercentage / 100m);

        var finalPremium = finalPremiumInBase / currency.ExchangeRateToBase;

        return Math.Round(finalPremium, 2);
    }
}
