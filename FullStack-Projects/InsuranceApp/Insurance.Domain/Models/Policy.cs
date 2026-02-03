using Insurance.Domain.Enums;

namespace Insurance.Domain.Models;

public class Policy
{
    public Guid Id { get; set; }

    public required string PolicyNumber { get; set; }

    public Guid BuildingId { get; set; }

    public required Building Building { get; set; }

    public Guid BrokerId { get; set; }

    public required Broker Broker { get; set; }

    public Guid CurrencyId { get; set; }

    public PolicyStatus Status { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal BasePremium { get; set; }

    public decimal FinalPremium { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancellationReason { get; set; }
}
