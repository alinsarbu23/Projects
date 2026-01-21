namespace Insurance.Application.DTOs.Clients;

public class GetClientByIdDto
{
    public Guid Id { get; init; }
    public string ClientType { get; set; }
    public string Name { get; init; }

    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public string Identifier { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public IReadOnlyList<ClientBuildingSummaryDto> Buildings { get; set; }
}
