using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Interfaces.Services
{
    public interface IClientService
    {
        Task<Guid> CreateClientAsync(CreateClientDto dto);
        Task UpdateClientAsync(Guid clientId, UpdateClientDto dto);
        Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto dto);
        Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId);
        /// <summary>
        /// Search by partial name and exact identifier (CNP or CUI).
        /// </summary>
        Task<PagedClientsDto> SearchClientsAsync(string? name, string? identifier, int page, int pageSize);
    }
}
