namespace Insurance.Application.DTOs.Clients;

public class CreateCompanyClientDto
{
    public required string CompanyName { get; init; }
    public required string Cui { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string Address { get; init; }
}
