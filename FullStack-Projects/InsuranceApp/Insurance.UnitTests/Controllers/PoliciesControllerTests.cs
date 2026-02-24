using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;
using Insurance.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Insurance.UnitTests.Controllers;
public class PoliciesControllerTests
{
    private readonly Mock<IPolicyService> _service;
    private readonly PoliciesController _controller;

    public PoliciesControllerTests()
    {
        _service = new Mock<IPolicyService>();
        _controller = new PoliciesController(_service.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        _service
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetPolicyByIdDto());

        var result = await _controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Search_ShouldCallService_WithCorrectParameters()
    {
        var service = new Mock<IPolicyService>();
        service.Setup(x => x.SearchAsync(
                It.IsAny<Guid?>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                1,
                20))
            .ReturnsAsync(new PagedPoliciesDto());

        var controller = new PoliciesController(service.Object);

        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.ControllerContext.HttpContext.User =
            new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Email, "broker@test.ro") }));

        var result = await controller.Search(null, null, null, null);

        result.Result.Should().BeOfType<OkObjectResult>();
        service.Verify(x => x.SearchAsync(
            null,
            "broker@test.ro",
            null,
            null,
            null,
            1,
            20), Times.Once);
    }

    [Fact]
    public async Task UpdatePolicy_ShouldReturnNoContent()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        var result = await controller.UpdatePolicy(Guid.NewGuid(), new UpdatePolicyDto());

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.UpdatePolicyAsync(
            It.IsAny<Guid>(),
            It.IsAny<UpdatePolicyDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePolicyAsync_WhenDraftPolicy_UpdatesAndRecalculatesPremium()
    {
        var policyId = Guid.NewGuid();
        var brokerId = Guid.NewGuid();
        var currencyId = Guid.NewGuid();

        var policy = new Policy
        {
            Id = policyId,
            BrokerId = brokerId,
            BuildingId = Guid.NewGuid(),
            CurrencyId = currencyId,
            Status = PolicyStatus.Draft,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10),
            BasePremium = 100,
            FinalPremium = 120,
            Building = new Building
            {
                Id = Guid.NewGuid(),
                City = new City
                {
                    County = new County
                    {
                        CountryId = Guid.NewGuid()
                    }
                }
            }
        };

        var uow = new Mock<IUnitOfWork>();
        var calc = new Mock<IPolicyCalculationService>();

        uow.Setup(x => x.Policies.GetByIdAsync(policyId))
            .ReturnsAsync(policy);

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = currencyId,
                Code = "RON",
                Name = "Romanian Leu",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        calc.Setup(x => x.CalculateFinalPremiumAsync(
                It.IsAny<decimal>(),
                brokerId,
                It.IsAny<Guid>(),
                policy.Building))
            .ReturnsAsync(200);

        uow.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var service = new PolicyService(
            uow.Object,
            calc.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        var dto = new UpdatePolicyDto
        {
            CurrencyId = currencyId,
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(20),
            BasePremium = 150
        };

        await service.UpdatePolicyAsync(policyId, dto);

        policy.BasePremium.Should().Be(150);
        policy.FinalPremium.Should().Be(200);
        policy.StartDate.Should().Be(dto.StartDate);
        policy.EndDate.Should().Be(dto.EndDate);

        uow.Verify(x => x.Policies.UpdateAsync(policy), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Activate_WhenCalled_ShouldReturnNoContent()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        var result = await controller.Activate(Guid.NewGuid());

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.ActivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Activate_WhenValidPolicyId_ShouldInvokeService()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        await controller.Activate(Guid.NewGuid());

        service.Verify(x => x.ActivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Cancel_ShouldReturnNoContent()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        var result = await controller.Cancel(Guid.NewGuid(), new DeletePolicyDto
        {
            Reason = "Client request"
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.CancelAsync(It.IsAny<Guid>(), It.IsAny<DeletePolicyDto>()), Times.Once);
    }
}
