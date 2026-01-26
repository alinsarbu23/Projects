using Insurance.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    private static readonly string DbName = $"InsuranceDb_IntegrationTests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<InsuranceDbContext>(options =>
            {
                options.UseSqlServer(
                    $"Server=(localdb)\\mssqllocaldb;Database={DbName};Trusted_Connection=True;MultipleActiveResultSets=true");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

            db.Database.EnsureCreated();
        });
    }
}
