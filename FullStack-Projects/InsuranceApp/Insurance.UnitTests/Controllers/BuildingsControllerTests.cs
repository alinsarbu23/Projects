using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;

public class BuildingsControllerTests
{
    [Fact]
    public async Task GetBuildingsForClient_ShouldReturnOk()
    {
        var service = new Mock<IBuildingService>();
        var clientId = Guid.NewGuid();

        var expected = new List<ClientBuildingSummaryDto>
        {
            new ClientBuildingSummaryDto
            {
                Id = Guid.NewGuid(),
                City = "Cluj-Napoca",
                BuildingType = "Residential",
                InsuredValue = 1000
            }
        };

        service.Setup(x => x.GetBuildingsForClientAsync(clientId)).ReturnsAsync(expected);

        var controller = new BuildingsController(service.Object);

        var actionResult = await controller.GetBuildingsForClient(clientId);

        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();

        var payload = ok!.Value as IReadOnlyList<ClientBuildingSummaryDto>;
        payload.Should().NotBeNull();
        payload![0].City.Should().Be("Cluj-Napoca");
        payload[0].BuildingType.Should().Be("Residential");
    }

    [Fact]
    public async Task GetBuildingById_ShouldReturnOk()
    {
        var service = new Mock<IBuildingService>();
        var buildingId = Guid.NewGuid();

        var expected = new GetBuildingByIdDto
        {
            Id = buildingId,
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100,
            FloodRisk = true,
            EarthquakeRisk = false
        };

        service.Setup(x => x.GetBuildingByIdAsync(buildingId))
            .ReturnsAsync(expected);

        var controller = new BuildingsController(service.Object);

        var actionResult = await controller.GetBuildingById(buildingId);

        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();

        var payload = ok!.Value as GetBuildingByIdDto;
        payload.Should().NotBeNull();
        payload!.FloodRisk.Should().BeTrue();
        payload.EarthquakeRisk.Should().BeFalse();
    }

    [Fact]
    public async Task CreateBuilding_ShouldReturnCreatedAt_WithBuildingId()
    {
        var service = new Mock<IBuildingService>();
        var controller = new BuildingsController(service.Object);

        var clientId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();

        service.Setup(x => x.CreateBuildingAsync(clientId, It.IsAny<CreateBuildingDto>())).ReturnsAsync(buildingId);

        var result = await controller.CreateBuilding(clientId, new CreateBuildingDto
        {
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = 50,
            InsuredValue = 1000,
            FloodRisk = true,
            EarthquakeRisk = false
        });

        var created = result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created!.RouteValues!["buildingId"].Should().Be(buildingId);
    }

    [Fact]
    public async Task UpdateBuilding_ShouldReturnNoContent()
    {
        var service = new Mock<IBuildingService>();
        var buildingId = Guid.NewGuid();

        service.Setup(x => x.UpdateBuildingAsync(buildingId, It.IsAny<UpdateBuildingDto>())).Returns(Task.CompletedTask);

        var controller = new BuildingsController(service.Object);

        var dto = new UpdateBuildingDto
        {
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = 50,
            InsuredValue = 1000,
            FloodRisk = true,
            EarthquakeRisk = true
        };

        var result = await controller.UpdateBuilding(buildingId, dto);

        result.Should().BeOfType<NoContentResult>();
    }
}
