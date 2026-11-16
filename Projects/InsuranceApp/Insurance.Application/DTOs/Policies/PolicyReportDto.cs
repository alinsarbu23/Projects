namespace Insurance.Application.DTOs.Policies;

public class PolicyReportDto
{
    public Guid BrokerId { get; set; }

    public string BrokerCode { get; set; } = string.Empty;

    public string BrokerName { get; set; } = string.Empty;

    public int PolicyCount { get; set; }

    public decimal TotalFinalPremium { get; set; }
}
