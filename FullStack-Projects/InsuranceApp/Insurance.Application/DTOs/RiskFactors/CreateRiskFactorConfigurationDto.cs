namespace Insurance.Application.DTOs.RiskFactors;

public class CreateRiskFactorConfigurationDto
{
    public string Level { get; set; } = string.Empty;

    public Guid ReferenceId { get; set; }

    public decimal AdjustmentPercentage { get; set; }
}
