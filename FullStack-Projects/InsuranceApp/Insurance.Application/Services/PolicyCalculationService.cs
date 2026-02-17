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
        _logger.LogInformation(
            "Calculating premium (BrokerId={BrokerId}, CurrencyId={CurrencyId}, BasePremium={BasePremium})",
            brokerId,
            currencyId,
            basePremium);

        decimal totalPercentage = 0m;

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning("Premium calculation failed. Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        if (broker.CommissionPercentage > 0)
        {
            totalPercentage += broker.CommissionPercentage;
        }

        var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);

        if (currency == null || !currency.IsActive)
        {
            _logger.LogWarning("Premium calculation failed. Invalid or inactive currency {CurrencyId}",
                currencyId);
            throw new InvalidOperationException("Invalid or inactive currency");
        }

        var premiumInBaseCurrency = basePremium * currency.ExchangeRateToBase;

        var fees = await _unitOfWork.FeeConfigurations
            .GetActiveAsync(null, DateTime.UtcNow);

        foreach (var fee in fees)
        {
            if (fee.Type == FeeType.RiskAdjustment)
            {
                if (fee.AppliesToRisk == BuildingRiskType.Flood && building.FloodRisk)
                    totalPercentage += fee.Percentage;

                if (fee.AppliesToRisk == BuildingRiskType.Earthquake && building.EarthquakeRisk)
                    totalPercentage += fee.Percentage;
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

        var rounded = Math.Round(finalPremium, 2);

        _logger.LogInformation(
            "Premium calculated successfully. FinalPremium={FinalPremium}",
            rounded);

        return rounded;
    }
}
