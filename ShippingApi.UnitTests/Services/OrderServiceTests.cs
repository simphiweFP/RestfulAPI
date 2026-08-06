using FluentAssertions;
using Moq;
using ShippingApi.Dtos.Order;
using ShippingApi.Models;
using ShippingApi.UseCase;
using Xunit;

namespace ShippingApi.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly ShippingApi.Services.OrderService _service;

    public OrderServiceTests()
    {
        _service = new ShippingApi.Services.OrderService(_orderRepository.Object);
    }

    [Fact]
    public async Task PlaceOrderAsync_AddsOrder_WithCalculatedTotalAndCopiedItems()
    {
        Order? capturedOrder = null;
        _orderRepository
            .Setup(r => r.AddOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        var items = new[]
        {
            new Item { Id = 1, Name = "Box", Price = 12.50m },
            new Item { Id = 2, Name = "Tape", Price = 3.25m }
        };

        await _service.PlaceOrderAsync(42, items);

        capturedOrder.Should().NotBeNull();
        capturedOrder!.UserId.Should().Be(42);
        capturedOrder.Items.Should().ContainEquivalentOf(items[0]);
        capturedOrder.Items.Should().ContainEquivalentOf(items[1]);
        capturedOrder.TotalAmount.Should().Be(15.75m);
        _orderRepository.Verify(r => r.AddOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUserOrdersAsync_FiltersOrdersForUser()
    {
        _orderRepository
            .Setup(r => r.GetOrdersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                new Order { Id = 1, UserId = 10, TotalAmount = 100m },
                new Order { Id = 2, UserId = 20, TotalAmount = 200m },
                new Order { Id = 3, UserId = 10, TotalAmount = 300m }
            });

        var result = await _service.GetUserOrdersAsync(10);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(order => order.UserId == 10);
    }

    [Fact]
    public async Task GetOrdersAsync_WithQuery_DelegatesToRepository()
    {
        var query = new OrderQueryParameters { PageNumber = 2, PageSize = 5, UserId = 10 };
        var pagedResult = new PagedResult<Order>(Array.Empty<Order>(), 0, 2, 5, 0);

        _orderRepository
            .Setup(r => r.GetOrdersAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        var result = await _service.GetOrdersAsync(query);

        result.Should().BeSameAs(pagedResult);
        _orderRepository.Verify(r => r.GetOrdersAsync(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsRepositoryOrder()
    {
        var expected = new Order { Id = 7, UserId = 1, TotalAmount = 19.99m };
        _orderRepository.Setup(r => r.GetOrderByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _service.GetOrderByIdAsync(7);

        result.Should().BeSameAs(expected);
    }
}
