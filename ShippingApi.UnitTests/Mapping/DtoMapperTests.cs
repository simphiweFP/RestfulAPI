using FluentAssertions;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Driver;
using ShippingApi.Dtos.Mapping;
using ShippingApi.Dtos.Order;
using ShippingApi.Models;
using Xunit;

namespace ShippingApi.UnitTests.Mapping;

public class DtoMapperTests
{
    [Fact]
    public void Driver_ToResponse_MapsNestedAddress()
    {
        var model = new Driver
        {
            Id = 1,
            Name = "Driver",
            Email = "driver@example.com",
            DriverNumber = 33,
            Team = "Blue",
            Address = new Address { Id = 7, Street = "Street", City = "City", ZipCode = "0001" }
        };

        var response = model.ToResponse();

        response.Id.Should().Be(1);
        response.Address.Should().NotBeNull();
        response.Address!.Street.Should().Be("Street");
    }

    [Fact]
    public void CreateDriverRequest_ToModel_MapsAddress()
    {
        var request = new CreateDriverRequest
        {
            Name = "Driver",
            Email = "driver@example.com",
            DriverNumber = 33,
            Team = "Blue",
            Address = new CreateAddressRequest { Street = "Street", City = "City", ZipCode = "0001" }
        };

        var model = request.ToModel();

        model.Address.Should().NotBeNull();
        model.Address!.City.Should().Be("City");
    }

    [Fact]
    public void CreateOrderRequest_ToModel_CalculatesTotalAmount()
    {
        var request = new CreateOrderRequest
        {
            UserId = 11,
            Items = new List<OrderItemRequest>
            {
                new() { Name = "ItemA", Price = 10m },
                new() { Name = "ItemB", Price = 5.5m }
            }
        };

        var model = request.ToModel();

        model.UserId.Should().Be(11);
        model.TotalAmount.Should().Be(15.5m);
        model.Items.Should().HaveCount(2);
    }

    [Fact]
    public void Order_ToResponse_MapsItems()
    {
        var model = new Order
        {
            Id = 2,
            UserId = 20,
            TotalAmount = 99.99m,
            Items = new[] { new Item { Id = 1, Name = "Parcel", Price = 99.99m } }
        };

        var response = model.ToResponse();

        response.Id.Should().Be(2);
        response.Items.Should().ContainSingle();
        response.Items.Single().Name.Should().Be("Parcel");
    }
}
