namespace Insurance.Application.Interfaces.Services;

using Insurance.Application.DTOs.Geography;

public interface IGeographyService
{
    Task<IReadOnlyList<GetCountriesDto>> GetCountriesAsync();
    Task<IReadOnlyList<GetCountiesByCountryDto>> GetCountiesByCountryAsync(Guid countryId);
    Task<IReadOnlyList<GetCitiesByCountyDto>> GetCitiesByCountyAsync(Guid countyId);
}
