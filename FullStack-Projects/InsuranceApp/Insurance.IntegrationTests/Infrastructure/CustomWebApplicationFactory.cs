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

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile(
                Path.Combine(AppContext.BaseDirectory, "appsettings.Testing.json"),
                optional: false);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<InsuranceDbContext>((sp, options) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                options.UseSqlServer(
                    configuration.GetConnectionString("InsuranceDb"));
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            db.Database.EnsureCreated();

            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            DatabaseReset.InitializeAsync(config).GetAwaiter().GetResult();

            TestDataSeeder.SeedAsync(db).GetAwaiter().GetResult();
        });
    }
}