namespace Insurance.Application.DTOs.Clients;

public class GetClientByIdDto
{
    public Guid Id { get; init; }
    public string ClientType { get; init; }
    public string Name { get; init; }

    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public string Identifier { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Address { get; init; }

    public IReadOnlyList<ClientBuildingSummaryDto> Buildings { get; init; }
}
