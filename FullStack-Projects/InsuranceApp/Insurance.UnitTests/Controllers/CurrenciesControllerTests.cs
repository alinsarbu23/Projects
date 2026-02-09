using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;
public class CurrenciesControllerTests
{
    private readonly Mock<ICurrencyService> _service;
    private readonly CurrenciesController _controller;

    public CurrenciesControllerTests()
    {
        _service = new Mock<ICurrencyService>();
        _controller = new CurrenciesController(_service.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt()
    {
        var id = Guid.NewGuid();

        _service
            .Setup(x => x.CreateAsync(It.IsAny<CreateCurrencyDto>()))
            .ReturnsAsync(id);

        var result = await _controller.Create(new CreateCurrencyDto
        {
            Code = "EUR",
            Name = "Euro",
            ExchangeRateToBase = 4.9m
        });

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _service
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<GetCurrenciesDto>());

        var result = await _controller.GetAll();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        var service = new Mock<ICurrencyService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetCurrencyByIdDto());

        var controller = new CurrenciesController(service.Object);

        var result = await controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        var service = new Mock<ICurrencyService>();
        var controller = new CurrenciesController(service.Object);

        var result = await controller.Update(Guid.NewGuid(), new UpdateCurrencyDto
        {
            Name = "EUR",
            ExchangeRateToBase = 4.9m,
            IsActive = 1
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateCurrencyDto>()), Times.Once);
    }
}
