namespace Insurance.Application.Interfaces.Repositories;

using Insurance.Domain.Models;

public interface IClientRepository
{
    Task AddIndividualAsync(IndividualClient client);
    Task AddCompanyAsync(CompanyClient client);

    Task<IndividualClient?> GetIndividualByIdAsync(Guid clientId);
    Task<CompanyClient?> GetCompanyByIdAsync(Guid clientId);

    Task<IReadOnlyList<IndividualClient>> SearchIndividualsAsync(string? name, string? cnp);

    Task<IReadOnlyList<CompanyClient>> SearchCompaniesAsync(string? name, string? cui);

    Task UpdateIndividualAsync(IndividualClient client);
    Task UpdateCompanyAsync(CompanyClient client);

    Task<bool> IndividualExistsByCnpAsync(string cnp);
    Task<bool> CompanyExistsByCuiAsync(string cui);
}
