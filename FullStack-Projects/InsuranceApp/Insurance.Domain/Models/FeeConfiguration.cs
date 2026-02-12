using Insurance.Domain.Enums;

namespace Insurance.Domain.Models;

public class FeeConfiguration
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public FeeType Type { get; set; }

    public decimal Percentage { get; set; }

    public BuildingRiskType? AppliesToRisk { get; set; }

    public bool IsActive { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }
}
