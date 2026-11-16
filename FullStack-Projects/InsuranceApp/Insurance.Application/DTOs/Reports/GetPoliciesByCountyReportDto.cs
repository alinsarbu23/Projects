namespace Insurance.Application.DTOs.Reports;

public class GetPoliciesByCountyReportDto
{
    public string CountyName { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public int PolicyCount { get; init; }

    public decimal TotalFinalPremium { get; init; }

    public decimal TotalFinalPremiumInBaseCurrency { get; init; }
}
