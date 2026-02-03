using Insurance.Domain.Models;

namespace Insurance.Infrastructure.DAOs;

public class Broker
{
    public Guid Id { get; set; }

    public string BrokerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal CommissionPercentage { get; set; }

    public string Status { get; set; } = string.Empty;

    public ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
