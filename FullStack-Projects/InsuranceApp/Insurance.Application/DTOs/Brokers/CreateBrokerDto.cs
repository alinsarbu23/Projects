namespace Insurance.Application.DTOs.Brokers;

public class CreateBrokerDto
{
    public string BrokerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public decimal CommissionPercentage { get; set; }
}
