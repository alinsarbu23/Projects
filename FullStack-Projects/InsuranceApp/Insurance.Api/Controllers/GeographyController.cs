using Insurance.Application.DTOs.Geography;
using Insurance.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/brokers")]
    public class GeographyController : ControllerBase
    {
        private readonly IGeographyService _geographyService;

        public GeographyController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<GetCountriesDto>>> GetCountries()
        {
            var result = await _geographyService.GetCountriesAsync();
            return Ok(result);
        }

        [HttpGet("countries/{countryId:guid}/counties")]
        public async Task<ActionResult<IReadOnlyList<GetCountiesByCountryDto>>> GetCountiesByCountry(Guid countryId)
        {
            var result = await _geographyService.GetCountiesByCountryAsync(countryId);
            return Ok(result);
        }

        [HttpGet("counties/{countyId:guid}/cities")]
        public async Task<ActionResult<IReadOnlyList<GetCitiesByCountyDto>>> GetCitiesByCounty(Guid countyId)
        {
            var result = await _geographyService.GetCitiesByCountyAsync(countyId);
            return Ok(result);
        }
    }
}
