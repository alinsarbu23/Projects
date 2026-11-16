using Insurance.Domain.Enums;

namespace Insurance.Domain.Models;

public class Broker
{
    public Guid Id { get; set; }

    public required string BrokerCode { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal CommissionPercentage { get; set; }

    public BrokerStatus Status { get; set; }
}
