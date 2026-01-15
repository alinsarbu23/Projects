namespace Insurance.Application.DTOs.Clients;

public class UpdateClientIdentifierDto
{
    public required string OldIdentifier { get; init; }
    public required string NewIdentifier { get; init; }
    public required string Reason { get; init; }
}
