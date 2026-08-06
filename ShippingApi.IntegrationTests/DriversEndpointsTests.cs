using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Core;
using ShippingApi.Data;
using ShippingApi.Dtos.Driver;
using ShippingApi.Dtos.Common;
using ShippingApi.IntegrationTests.Infrastructure;
using ShippingApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ShippingApi.IntegrationTests;

public class DriversEndpointsTests
{
    private readonly ShippingApiWebApplicationFactory _factory = new();
    private readonly HttpClient _client;

    public DriversEndpointsTests()
    {
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        SeedData();
    }

    [Fact]
    public async Task GetDrivers_ReturnsSeededDrivers()
    {
        var response = await _client.GetAsync("/api/drivers");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<PagedResponse<DriverResponse>>();

        body.Should().NotBeNull();
        body!.Items.Should().ContainSingle();
        body.Items.First().Name.Should().Be("Driver One");
    }

    [Fact]
    public async Task CreateDriver_ReturnsCreatedDriver()
    {
        var request = new CreateDriverRequest
        {
            Name = "New Driver",
            Email = "new.driver@example.com",
            DriverNumber = 88,
            Team = "Blue"
        };

        var response = await _client.PostAsJsonAsync("/api/drivers", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<DriverResponse>();

        body.Should().NotBeNull();
        body!.Name.Should().Be("New Driver");
    }

    [Fact]
    public async Task DeleteDriver_RemovesDriver()
    {
        var response = await _client.DeleteAsync("/api/drivers/1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await context.Drivers.FindAsync(1)).Should().BeNull();
    }

    private void SeedData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var address = new Address
        {
            Id = 1,
            Street = "1 Main Street",
            City = "Johannesburg",
            ZipCode = "2000"
        };
        context.Address.Add(address);

        context.Drivers.Add(new Driver
        {
            Id = 1,
            Name = "Driver One",
            Email = "driver.one@example.com",
            DriverNumber = 10,
            Team = "Red",
            Address = address
        });

        context.SaveChanges();
    }
}
