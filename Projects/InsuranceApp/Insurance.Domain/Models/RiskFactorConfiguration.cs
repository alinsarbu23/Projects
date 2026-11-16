using Insurance.Domain.Enums;

namespace Insurance.Domain.Models;

public class RiskFactorConfiguration
{
    public Guid Id { get; set; }

    public RiskLevel Level { get; set; }

    public Guid ReferenceId { get; set; }

    public decimal AdjustmentPercentage { get; set; }

    public bool IsActive { get; set; }
}
