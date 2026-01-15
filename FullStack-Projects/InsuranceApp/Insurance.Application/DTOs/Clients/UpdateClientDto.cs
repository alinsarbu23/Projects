namespace Insurance.Application.DTOs.Clients;

public class UpdateClientDto
{
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string Address { get; init; }
}
