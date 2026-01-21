namespace Insurance.Application.DTOs.Clients;

public class GetClientsDto
{
    public Guid Id { get; set; }
    public string ClientType { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public string Identifier { get; set; }
}
