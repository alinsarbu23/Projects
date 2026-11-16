namespace Insurance.Application.DTOs.Brokers;

public class GetBrokerByIdDto
{
    public Guid Id { get; set; }

    public string BrokerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal CommissionPercentage { get; set; }
}
