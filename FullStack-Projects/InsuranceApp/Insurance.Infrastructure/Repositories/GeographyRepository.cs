using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models.Geography;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;
public class GeographyRepository : IGeographyRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public GeographyRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<Country>> GetAllCountriesAsync()
    {
        var countriesDAO = await _context.Countries.AsNoTracking().OrderBy(country => country.Name).ToListAsync();
        return _mapper.Map<IReadOnlyList<Country>>(countriesDAO);
    }

    public async Task<IReadOnlyList<City>> GetCitiesByCountyIdAsync(Guid countyId)
    {
        var citiesDAO = await _context.Cities.AsNoTracking().Where(city => city.CountyId == countyId).OrderBy(city => city.Name).ToListAsync();
        return _mapper.Map<IReadOnlyList<City>>(citiesDAO);
    }

    public async Task<IReadOnlyList<County>> GetCountiesByCountryIdAsync(Guid countryId)
    {
        var countiesDAO = await _context.Counties.AsNoTracking().Where(county => county.CountryId == countryId).ToListAsync();
        return _mapper.Map<IReadOnlyList<County>>(countiesDAO);
    }
}
