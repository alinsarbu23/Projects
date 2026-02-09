using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Services;

public interface IPolicyCalculationService
{
    Task<decimal> CalculateFinalPremiumAsync(decimal basePremium, Guid brokerId, Guid currencyId, Building building);
}
