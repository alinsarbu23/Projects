namespace Insurance.Application.DTOs.Clients;

public class GetClientByIdDto
{
    public Guid Id { get; init; }

    public required string ClientType { get; set; }

    public required string Name { get; init; }


    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public required string Identifier { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }

    public required string Address { get; set; }

    public required IReadOnlyList<ClientBuildingSummaryDto> Buildings { get; set; }
}
