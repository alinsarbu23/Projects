using Insurance.Domain.Models.Geography;

namespace Insurance.Application.Interfaces.Repositories;
public interface IGeographyRepository
{
    Task<IReadOnlyList<Country>> GetAllCountriesAsync();
    Task<IReadOnlyList<County>> GetCountiesByCountryIdAsync(Guid countryId);
    Task<IReadOnlyList<City>> GetCitiesByCountyIdAsync(Guid countyId);
}

