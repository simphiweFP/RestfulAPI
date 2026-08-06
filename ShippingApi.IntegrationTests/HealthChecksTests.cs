using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ShippingApi.IntegrationTests.Infrastructure;
using Xunit;

namespace ShippingApi.IntegrationTests;

public class HealthChecksTests
{
    private readonly ShippingApiWebApplicationFactory _factory = new();
    private readonly HttpClient _client;

    public HealthChecksTests()
    {
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health");

        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
