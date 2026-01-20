namespace Insurance.Application.DTOs.Clients;

public class CreateClientDto
{
    public required string ClientType { get; init; } 

    public required string Name { get; init; }

    public string? Cnp { get; init; }
    public string? Cui { get; init; }

    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string Address { get; init; }
}
