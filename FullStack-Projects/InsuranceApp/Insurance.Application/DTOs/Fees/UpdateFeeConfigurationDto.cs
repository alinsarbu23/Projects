namespace Insurance.Application.DTOs.Fees;

public class UpdateFeeConfigurationDto
{
    public decimal Percentage { get; set; }

    public int IsActive { get; set; }
}
