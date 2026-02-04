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
        Building building)
    {
        decimal totalPercentage = 0m;

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if (broker == null)
        {
            _logger.LogError("Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        if (broker.CommissionPercentage > 0)
        {
            totalPercentage += broker.CommissionPercentage;
        }

        var fees = await _unitOfWork.FeeConfigurations.GetActiveAsync(null, DateTime.UtcNow);

        foreach (var fee in fees)
        {
            if (fee.Type == FeeType.RiskAdjustment)
            {
                var applies =
                    fee.AppliesToRisk == "Earthquake" && building.EarthquakeRisk ||
                    fee.AppliesToRisk == "Flood" && building.FloodRisk;

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


        var risks = await _unitOfWork.RiskFactorConfigurations.GetActiveForBuildingAsync(
            building.City.County.CountryId,
            building.City.CountyId,
            building.CityId,
            building.BuildingTypeId);

        foreach (var risk in risks)
        {
            totalPercentage += risk.AdjustmentPercentage;
        }

        var finalPremium = basePremium * (1 + totalPercentage / 100m);
        return Math.Round(finalPremium, 2);
    }
}
