namespace Insurance.Application.DTOs.Reports;

public class GetPoliciesByBrokerReportDto
{
    public Guid BrokerId { get; init; }

    public string BrokerCode { get; init; } = string.Empty;

    public string BrokerName { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public int PolicyCount { get; init; }

    public decimal TotalFinalPremium { get; init; }

    public decimal TotalFinalPremiumInBaseCurrency { get; init; }
}
