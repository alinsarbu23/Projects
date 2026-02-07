namespace Insurance.Application.DTOs.Clients;

public class GetClientsDto
{
    public Guid Id { get; set; }

    public required string ClientType { get; set; }

    public required string Name { get; set; }


    /// <summary>
    /// CNP for INDIVIDUAL, CUI for COMPANY
    /// </summary>
    public required string Identifier { get; set; }

}
