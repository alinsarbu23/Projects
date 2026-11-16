namespace Insurance.Application.DTOs.Fees;

public class CreateFeeConfigurationDto
{
    public string Name { get; set; } = string.Empty;

    public int Type { get; set; }

    public decimal Percentage { get; set; }

    public int AppliesToRisk { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }
}
