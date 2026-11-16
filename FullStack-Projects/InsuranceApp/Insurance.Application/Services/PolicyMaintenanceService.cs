using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class PolicyMaintenanceService : IPolicyMaintenanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PolicyMaintenanceService> _logger;

    public PolicyMaintenanceService(IUnitOfWork unitOfWork, ILogger<PolicyMaintenanceService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExpirePoliciesAsync()
    {
        var today = DateTime.UtcNow.Date;

        var affectedRows = await _unitOfWork.Policies.ExpireActivePoliciesAsync(today);

        if (affectedRows == 0)
        {
            _logger.LogInformation("No policies to expire.");
            return;
        }

        _logger.LogInformation("Expired {Count} policies automatically.", affectedRows);
    }
}
