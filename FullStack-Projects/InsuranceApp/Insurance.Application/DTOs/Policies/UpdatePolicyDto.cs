namespace Insurance.Application.DTOs.Policies;

public class UpdatePolicyDto
{
    public Guid CurrencyId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal BasePremium { get; set; }
}
