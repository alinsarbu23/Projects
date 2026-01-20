namespace Insurance.Application.DTOs.Clients;

public class GetClientsDto
{
    public Guid Id { get; init; }
    public string ClientType { get; init; }
    public string Name { get; init; }

    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public string Identifier { get; init; }
}
