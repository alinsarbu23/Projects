using AutoMapper;
using Insurance.Application.DTOs.Geography;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class GeographyService : IGeographyService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GeographyService> _logger;

    public GeographyService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<GeographyService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IReadOnlyList<GetCitiesByCountyDto>> GetCitiesByCountyAsync(Guid countyId)
    {
        _logger.LogInformation("Retrieving cities for County {CountyId}", countyId);

        var cities = await _unitOfWork.Geography.GetCitiesByCountyIdAsync(countyId);

        _logger.LogInformation(
            "Retrieved {Count} cities for County {CountyId}",
            cities.Count,
            countyId);

        return _mapper.Map<IReadOnlyList<GetCitiesByCountyDto>>(cities);
    }

    public async Task<IReadOnlyList<GetCountiesByCountryDto>> GetCountiesByCountryAsync(Guid countryId)
    {
        _logger.LogInformation("Retrieving counties for Country {CountryId}", countryId);

        var counties = await _unitOfWork.Geography.GetCountiesByCountryIdAsync(countryId);

        _logger.LogInformation(
            "Retrieved {Count} counties for Country {CountryId}",
            counties.Count,
            countryId);

        return _mapper.Map<IReadOnlyList<GetCountiesByCountryDto>>(counties);
    }

    public async Task<IReadOnlyList<GetCountriesDto>> GetCountriesAsync()
    {
        _logger.LogInformation("Retrieving all countries");

        var countries = await _unitOfWork.Geography.GetAllCountriesAsync();

        _logger.LogInformation("Retrieved {Count} countries", countries.Count);

        return _mapper.Map<IReadOnlyList<GetCountriesDto>>(countries);
    }
}
