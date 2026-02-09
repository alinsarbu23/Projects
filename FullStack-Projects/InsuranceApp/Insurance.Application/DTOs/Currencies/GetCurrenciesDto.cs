namespace Insurance.Application.DTOs.Currencies;

public class GetCurrenciesDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
