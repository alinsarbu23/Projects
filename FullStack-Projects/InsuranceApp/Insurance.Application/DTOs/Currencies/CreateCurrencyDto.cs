namespace Insurance.Application.DTOs.Currencies;

public class CreateCurrencyDto
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal ExchangeRateToBase { get; set; }
}
