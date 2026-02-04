namespace Insurance.Application.DTOs.Fees;

public class UpdateFeeConfigurationDto
{
    public decimal Percentage { get; set; }

    public bool IsActive { get; set; }
}
