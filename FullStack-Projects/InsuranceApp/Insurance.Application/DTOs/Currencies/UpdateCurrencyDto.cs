namespace Insurance.Application.DTOs.Currencies;

public class UpdateCurrencyDto
{
    public string Name { get; set; } = string.Empty;

    public decimal ExchangeRateToBase { get; set; }

    public int IsActive { get; set; } 
}
