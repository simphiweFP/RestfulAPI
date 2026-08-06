using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ShippingApi.IntegrationTests.Infrastructure;
using Xunit;

namespace ShippingApi.IntegrationTests;

public class SwaggerTests
{
    private readonly ShippingApiWebApplicationFactory _factory;
    private readonly ISwaggerProvider _swaggerProvider;

    public SwaggerTests()
    {
        _factory = new ShippingApiWebApplicationFactory();
        _swaggerProvider = _factory.Services.GetRequiredService<ISwaggerProvider>();
    }

    [Fact]
    public void SwaggerDocument_ContainsApiPaths()
    {
        var document = _swaggerProvider.GetSwagger("v1");

        document.Should().NotBeNull();
        document.Paths.Keys.Should().Contain("/api/orders");
        document.Components.SecuritySchemes.Should().ContainKey("Bearer");
    }
}
