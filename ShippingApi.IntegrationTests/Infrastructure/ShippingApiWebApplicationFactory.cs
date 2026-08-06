using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShippingApi.Data;

namespace ShippingApi.IntegrationTests.Infrastructure;

public class ShippingApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"ShippingApiTests_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(_databaseName));

            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                options.DefaultForbidScheme = TestAuthHandler.SchemeName;
            });

            services.PostConfigure<AuthorizationOptions>(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(TestAuthHandler.SchemeName)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        });
    }
}
