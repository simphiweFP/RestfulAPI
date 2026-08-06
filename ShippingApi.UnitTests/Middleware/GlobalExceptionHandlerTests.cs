using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using ShippingApi.Middleware;
using Xunit;

namespace ShippingApi.UnitTests.Middleware;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task InvokeAsync_ReturnsSafeProblemDetailsWithoutExceptionMessage()
    {
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "trace-123";
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var middleware = new GlobalExceptionHandler(
            _ => throw new InvalidOperationException("database password leaked"),
            NullLogger<GlobalExceptionHandler>.Instance);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        responseBody.Position = 0;
        using var document = await JsonDocument.ParseAsync(responseBody);
        var root = document.RootElement;
        root.GetProperty("detail").GetString().Should().NotContain("database password leaked");
        root.GetProperty("traceId").GetString().Should().Be("trace-123");
    }
}
