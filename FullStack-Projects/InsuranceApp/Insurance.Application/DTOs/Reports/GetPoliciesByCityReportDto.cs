namespace Insurance.Application.DTOs.Reports;

public class GetPoliciesByCityReportDto
{
    public string CityName { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public int PolicyCount { get; init; }

    public decimal TotalFinalPremium { get; init; }

    public decimal TotalFinalPremiumInBaseCurrency { get; init; }
}
