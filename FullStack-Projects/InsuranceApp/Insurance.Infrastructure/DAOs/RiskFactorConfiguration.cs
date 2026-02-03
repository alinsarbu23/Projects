namespace Insurance.Infrastructure.DAOs;

public class RiskFactorConfiguration
{
    public Guid Id { get; set; }

    public string Level { get; set; } = string.Empty;

    public Guid ReferenceId { get; set; }

    public decimal AdjustmentPercentage { get; set; }

    public bool IsActive { get; set; }
}
