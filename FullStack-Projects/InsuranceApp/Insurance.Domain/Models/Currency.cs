namespace Insurance.Domain.Models;

public class Currency
{
    public Guid Id { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public decimal ExchangeRateToBase { get; set; }
    public bool IsActive { get; set; }
}
