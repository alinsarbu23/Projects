namespace Insurance.Infrastructure.DAOs;

public class Currency
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal ExchangeRateToBase { get; set; }

    public bool IsActive { get; set; }
}
