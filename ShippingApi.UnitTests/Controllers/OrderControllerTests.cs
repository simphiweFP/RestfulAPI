using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Controllers;
using ShippingApi.Dtos.Common;
using ShippingApi.Dtos.Order;
using ShippingApi.Models;
using ShippingApi.Services;
using Xunit;

namespace ShippingApi.UnitTests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _orderService = new();
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _controller = new OrderController(_orderService.Object);
    }

    [Fact]
    public async Task GetOrders_ReturnsPagedResponse()
    {
        var query = new OrderQueryParameters { PageNumber = 2, PageSize = 1, UserId = 7 };
        var pagedOrders = new PagedResult<Order>(new[]
        {
            new Order { Id = 12, UserId = 7, TotalAmount = 55m, Items = new[] { new Item { Id = 1, Name = "Envelope", Price = 55m } } }
        }, 1, 2, 1, 1);

        _orderService.Setup(s => s.GetOrdersAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(pagedOrders);

        var result = await _controller.GetOrders(query, CancellationToken.None);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<PagedResponse<OrderResponse>>().Subject;
        response.TotalCount.Should().Be(1);
        response.PageNumber.Should().Be(2);
        response.Items.Should().ContainSingle();
        response.Items.Single().Id.Should().Be(12);
        response.Items.Single().UserId.Should().Be(7);
    }

    [Fact]
    public async Task GetOrder_WhenMissing_ReturnsNotFound()
    {
        _orderService.Setup(s => s.GetOrderByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

        var result = await _controller.GetOrder(5, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreatedAtActionWithMappedResponse()
    {
        Order? captured = null;
        _orderService
            .Setup(s => s.AddOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) =>
            {
                order.Id = 42;
                captured = order;
            })
            .Returns(Task.CompletedTask);

        var request = new CreateOrderRequest
        {
            UserId = 99,
            Items = new List<OrderItemRequest>
            {
                new() { Name = "Label", Price = 9.50m }
            }
        };

        var result = await _controller.CreateOrder(request, CancellationToken.None);

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(OrderController.GetOrder));
        created.RouteValues!["id"].Should().Be(42);
        created.Value.Should().BeOfType<OrderResponse>().Which.UserId.Should().Be(99);
        captured.Should().NotBeNull();
        captured!.TotalAmount.Should().Be(9.50m);
    }

    [Fact]
    public async Task UpdateOrder_WhenRouteIdDiffers_ReturnsBadRequest()
    {
        var result = await _controller.UpdateOrder(1, new UpdateOrderRequest { Id = 2, UserId = 1, Items = new List<OrderItemRequest>() }, CancellationToken.None);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task UpdateOrder_WhenOrderMissing_ReturnsNotFound()
    {
        _orderService.Setup(s => s.GetOrderByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

        var result = await _controller.UpdateOrder(1, new UpdateOrderRequest
        {
            Id = 1,
            UserId = 1,
            Items = new List<OrderItemRequest> { new() { Name = "Item", Price = 1m } }
        }, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteOrder_WhenOrderMissing_ReturnsNotFound()
    {
        _orderService.Setup(s => s.GetOrderByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

        var result = await _controller.DeleteOrder(3, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }
}
