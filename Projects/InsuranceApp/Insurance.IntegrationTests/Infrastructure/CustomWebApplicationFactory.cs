using Insurance.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices((context, services) =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connectionString =
                context.Configuration.GetConnectionString("InsuranceDb");

            services.AddDbContext<InsuranceDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        });
    }
}
