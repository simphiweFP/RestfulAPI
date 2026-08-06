using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Dtos.Common;
using ShippingApi.Dtos.Order;
using ShippingApi.IntegrationTests.Infrastructure;
using ShippingApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ShippingApi.IntegrationTests;

public class OrdersEndpointsTests
{
    private readonly ShippingApiWebApplicationFactory _factory = new();
    private readonly HttpClient _client;

    public OrdersEndpointsTests()
    {
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        SeedData();
    }

    [Fact]
    public async Task GetOrders_ReturnsPagedFilteredResults()
    {
        var response = await _client.GetAsync("/api/orders?pageNumber=1&pageSize=2&userId=101");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<PagedResponse<OrderResponse>>();

        body.Should().NotBeNull();
        body!.TotalCount.Should().Be(2);
        body.Items.Should().HaveCount(2);
        body.Items.Should().OnlyContain(order => order.UserId == 101);
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreatedOrderResponse()
    {
        var request = new CreateOrderRequest
        {
            UserId = 303,
            Items = new List<OrderItemRequest>
            {
                new() { Name = "Label", Price = 8.75m },
                new() { Name = "Packing", Price = 3.25m }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();

        body.Should().NotBeNull();
        body!.UserId.Should().Be(303);
        body.TotalAmount.Should().Be(12.00m);
        body.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsNoContentAndPersistsChanges()
    {
        var request = new UpdateOrderRequest
        {
            Id = 1,
            UserId = 101,
            Items = new List<OrderItemRequest>
            {
                new() { Name = "Updated", Price = 42m }
            }
        };

        var response = await _client.PutAsJsonAsync("/api/orders/1", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var updated = await context.Orders.AsNoTracking().FirstAsync(order => order.Id == 1);
        updated.TotalAmount.Should().Be(42m);
        updated.UserId.Should().Be(101);
    }

    [Fact]
    public async Task DeleteOrder_RemovesOrder()
    {
        var response = await _client.DeleteAsync("/api/orders/2");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await context.Orders.FindAsync(2)).Should().BeNull();
    }

    private void SeedData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Orders.AddRange(
            new Order { Id = 1, UserId = 101, TotalAmount = 10m, Items = new List<Item>() },
            new Order { Id = 2, UserId = 101, TotalAmount = 20m, Items = new List<Item>() },
            new Order { Id = 3, UserId = 202, TotalAmount = 30m, Items = new List<Item>() });

        context.SaveChanges();
    }
}
