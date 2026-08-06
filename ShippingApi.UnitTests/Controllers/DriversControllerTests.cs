using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Controllers;
using ShippingApi.Core;
using ShippingApi.Dtos.Driver;
using ShippingApi.Dtos.Common;
using ShippingApi.Models;
using Xunit;

namespace ShippingApi.UnitTests.Controllers;

public class DriversControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IDriverRepository> _driverRepository = new();
    private readonly DriversController _controller;

    public DriversControllerTests()
    {
        _unitOfWork.SetupGet(u => u.Drivers).Returns(_driverRepository.Object);
        _unitOfWork.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _controller = new DriversController(_unitOfWork.Object);
    }

    [Fact]
    public async Task GetDrivers_ReturnsDriverResponses()
    {
        _driverRepository.Setup(r => r.GetDriversAsync(It.IsAny<DriverQueryParameters>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            new PagedResult<Driver>(new[]
            {
                new Driver { Id = 1, Name = "A", Email = "a@example.com", DriverNumber = 11, Team = "Team A" }
            }, 1, 1, 10, 1));

        var result = await _controller.GetDrivers(new DriverQueryParameters(), CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<PagedResponse<DriverResponse>>();
    }

    [Fact]
    public async Task GetDriver_WhenMissing_ReturnsNotFound()
    {
        _driverRepository.Setup(r => r.FindById(10, It.IsAny<CancellationToken>())).ReturnsAsync((Driver?)null);

        var result = await _controller.GetDriver(10, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateDriver_ReturnsCreatedAtAction()
    {
        Driver? captured = null;
        _driverRepository
            .Setup(r => r.Add(It.IsAny<Driver>(), It.IsAny<CancellationToken>()))
            .Callback<Driver, CancellationToken>((driver, _) =>
            {
                driver.Id = 8;
                captured = driver;
            })
            .ReturnsAsync(true);

        var request = new CreateDriverRequest
        {
            Name = "Driver One",
            Email = "driver@example.com",
            DriverNumber = 77,
            Team = "Alpha"
        };

        var result = await _controller.CreateDriver(request, CancellationToken.None);

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.RouteValues!["id"].Should().Be(8);
        created.Value.Should().BeOfType<DriverResponse>().Which.Name.Should().Be("Driver One");
        captured.Should().NotBeNull();
        _unitOfWork.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDriver_WhenRouteIdDiffers_ReturnsBadRequest()
    {
        var result = await _controller.UpdateDriver(1, new UpdateDriverRequest { Id = 2, Name = "N", Email = "e@example.com", DriverNumber = 1, Team = "T" }, CancellationToken.None);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteDriver_WhenMissing_ReturnsNotFound()
    {
        _driverRepository.Setup(r => r.FindById(3, It.IsAny<CancellationToken>())).ReturnsAsync((Driver?)null);

        var result = await _controller.DeleteDriver(3, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }
}
