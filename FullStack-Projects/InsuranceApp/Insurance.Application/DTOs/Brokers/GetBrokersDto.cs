namespace Insurance.Application.DTOs.Brokers;

public class GetBrokersDto
{
    public Guid Id { get; set; }

    public string BrokerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
