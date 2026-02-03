namespace Insurance.Infrastructure.DAOs;

public class Policy
{
    public Guid Id { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = null!;
    public Guid BrokerId { get; set; }
    public Broker Broker { get; set; } = null!;
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;

    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal BasePremium { get; set; }
    public decimal FinalPremium { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
}
