using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Common;
using ShippingApi.IntegrationTests.Infrastructure;
using ShippingApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ShippingApi.IntegrationTests;

public class AddressesEndpointsTests
{
    private readonly ShippingApiWebApplicationFactory _factory = new();
    private readonly HttpClient _client;

    public AddressesEndpointsTests()
    {
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        SeedData();
    }

    [Fact]
    public async Task GetAddresses_ReturnsSeededAddresses()
    {
        var response = await _client.GetAsync("/api/addresses");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<PagedResponse<AddressResponse>>();

        body.Should().NotBeNull();
        body!.Items.Should().ContainSingle();
        body.Items.First().Street.Should().Be("1 Main Street");
    }

    [Fact]
    public async Task CreateAddress_ReturnsCreatedAddress()
    {
        var request = new CreateAddressRequest
        {
            Street = "2 Market Street",
            City = "Cape Town",
            ZipCode = "8001"
        };

        var response = await _client.PostAsJsonAsync("/api/addresses", request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<AddressResponse>();

        body.Should().NotBeNull();
        body!.City.Should().Be("Cape Town");
    }

    [Fact]
    public async Task DeleteAddress_RemovesAddress()
    {
        var response = await _client.DeleteAsync("/api/addresses/1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await context.Address.FindAsync(1)).Should().BeNull();
    }

    private void SeedData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Address.Add(new Address
        {
            Id = 1,
            Street = "1 Main Street",
            City = "Johannesburg",
            ZipCode = "2000"
        });

        context.SaveChanges();
    }
}
