namespace Insurance.Application.DTOs.Brokers;

/// <summary>
/// Email remains the logical key between Broker and AspNetusers. Can not be changed.
/// </summary>
public class UpdateBrokerDto
{
    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public decimal CommissionPercentage { get; set; }
}
