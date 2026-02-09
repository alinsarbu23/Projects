namespace Insurance.Application.DTOs.Fees;

public class CreateFeeConfigurationDto
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public decimal Percentage { get; set; }

    public string? AppliesToRisk { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }
}
