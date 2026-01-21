using AutoMapper;
using Insurance.Application.DTOs.Geography;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;

namespace Insurance.Application.Services;

public class GeographyService : IGeographyService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public GeographyService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<GetCitiesByCountyDto>> GetCitiesByCountyAsync(Guid countyId)
    {
        var contries = await _unitOfWork.Geography.GetCitiesByCountyIdAsync(countyId);
        return _mapper.Map<IReadOnlyList<GetCitiesByCountyDto>>(contries);
    }

    public async Task<IReadOnlyList<GetCountiesByCountryDto>> GetCountiesByCountryAsync(Guid countryId)
    {
        var counties = await _unitOfWork.Geography.GetCountiesByCountryIdAsync(countryId);
        return _mapper.Map<IReadOnlyList<GetCountiesByCountryDto>>(counties);
    }

    public async Task<IReadOnlyList<GetCountriesDto>> GetCountriesAsync()
    {
        var countries = await _unitOfWork.Geography.GetAllCountriesAsync();
        return _mapper.Map<IReadOnlyList<GetCountriesDto>>(countries);
    }
}
