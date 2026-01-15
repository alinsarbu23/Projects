using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/brokers")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }


        [HttpGet("clients/{clientId:guid}/buildings")]
        public async Task<ActionResult<IReadOnlyList<ClientBuildingSummaryDto>>> GetBuildingsForClient(Guid clientId)
        {
            var buildings = await _buildingService.GetBuildingsForClientAsync(clientId);
            return Ok(buildings);
        }


        [HttpGet("buildings/{buildingId:guid}")]
        public async Task<ActionResult<GetBuildingByIdDto>> GetBuildingById(Guid buildingId)
        {
            var building = await _buildingService.GetBuildingByIdAsync(buildingId);
            return Ok(building);
        }

        [HttpPost("clients/{clientId:guid}/buildings")]
        public async Task<IActionResult> CreateBuilding(Guid clientId, [FromBody] CreateBuildingDto dto)
        {
            var buildingId = await _buildingService.CreateBuildingAsync(clientId, dto);
            return CreatedAtAction(nameof(GetBuildingById), new { buildingId }, null);
        }

        [HttpPut("buildings/{buildingId:guid}")]
        public async Task<IActionResult> UpdateBuilding(Guid buildingId, [FromBody] UpdateBuildingDto dto)
        {
            await _buildingService.UpdateBuildingAsync(buildingId, dto);
            return NoContent();
        }
    }
}
