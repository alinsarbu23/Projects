namespace Insurance.Application.DTOs.Clients;

public class CreateIndividualClientDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Cnp { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string Address { get; init; }
}
