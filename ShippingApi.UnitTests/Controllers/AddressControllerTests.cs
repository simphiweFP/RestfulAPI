using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Controllers;
using ShippingApi.Core;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Common;
using ShippingApi.Models;
using Xunit;

namespace ShippingApi.UnitTests.Controllers;

public class AddressControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly AddressController _controller;

    public AddressControllerTests()
    {
        _unitOfWork.SetupGet(u => u.Address).Returns(_addressRepository.Object);
        _unitOfWork.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _controller = new AddressController(_unitOfWork.Object);
    }

    [Fact]
    public async Task GetAddresses_ReturnsAddressResponses()
    {
        _addressRepository.Setup(r => r.GetAddressesAsync(It.IsAny<AddressQueryParameters>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            new PagedResult<Address>(new[]
            {
                new Address { Id = 1, Street = "1 Main", City = "Town", ZipCode = "12345" }
            }, 1, 1, 10, 1));

        var result = await _controller.GetAddresses(new AddressQueryParameters(), CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<PagedResponse<AddressResponse>>();
    }

    [Fact]
    public async Task GetAddress_WhenMissing_ReturnsNotFound()
    {
        _addressRepository.Setup(r => r.FindById(10, It.IsAny<CancellationToken>())).ReturnsAsync((Address?)null);

        var result = await _controller.GetAddress(10, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateAddress_ReturnsCreatedAtAction()
    {
        Address? captured = null;
        _addressRepository
            .Setup(r => r.Add(It.IsAny<Address>(), It.IsAny<CancellationToken>()))
            .Callback<Address, CancellationToken>((address, _) =>
            {
                address.Id = 9;
                captured = address;
            })
            .ReturnsAsync(true);

        var request = new CreateAddressRequest { Street = "10 Downing", City = "London", ZipCode = "SW1A" };

        var result = await _controller.CreateAddress(request, CancellationToken.None);

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.RouteValues!["id"].Should().Be(9);
        created.Value.Should().BeOfType<AddressResponse>().Which.City.Should().Be("London");
        captured.Should().NotBeNull();
        _unitOfWork.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAddress_WhenRouteIdDiffers_ReturnsBadRequest()
    {
        var result = await _controller.UpdateAddress(1, new UpdateAddressRequest { Id = 2, Street = "1", City = "2", ZipCode = "3" }, CancellationToken.None);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteAddress_WhenMissing_ReturnsNotFound()
    {
        _addressRepository.Setup(r => r.FindById(3, It.IsAny<CancellationToken>())).ReturnsAsync((Address?)null);

        var result = await _controller.DeleteAddress(3, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }
}
