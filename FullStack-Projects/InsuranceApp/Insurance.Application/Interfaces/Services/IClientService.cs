using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Interfaces.Services
{
    public interface IClientService
    {
        Task<Guid> CreateIndividualClientAsync(CreateIndividualClientDto dto);
        Task<Guid> CreateCompanyClientAsync(CreateCompanyClientDto dto);
        Task UpdateClientAsync(Guid clientId, UpdateClientDto dto);
        Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto dto);
        Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId);

        /// <summary>
        /// For Search with partial name and exact identifier.
        /// </summary>
        Task<IReadOnlyList<GetClientsDto>> SearchClientsAsync(string? name, string? identifier);

    }
}
