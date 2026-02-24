using Insurance.Application.Interfaces.Services;

namespace Insurance.Api.BackgroundServices;

public class PolicyBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PolicyBackgroundService> _logger;

    public PolicyBackgroundService(IServiceScopeFactory scopeFactory, ILogger<PolicyBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task ExecuteExpirationAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPolicyMaintenanceService>();

        await service.ExpirePoliciesAsync();

        _logger.LogInformation("Expiration job executed successfully.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Policy expiration job started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(1);
            var delay = nextRun - now;

            _logger.LogInformation("Next expiration run scheduled at {NextRun}", nextRun);

            try
            {
                await Task.Delay(delay, stoppingToken);

                await ExecuteExpirationAsync();
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while executing expiration job.");
            }
        }
    }
}
