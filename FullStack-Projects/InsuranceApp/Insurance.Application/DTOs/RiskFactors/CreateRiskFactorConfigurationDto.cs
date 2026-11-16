namespace Insurance.Application.DTOs.RiskFactors;

public class CreateRiskFactorConfigurationDto
{
    public int Level { get; set; }

    public Guid ReferenceId { get; set; }

    public decimal AdjustmentPercentage { get; set; }
}
