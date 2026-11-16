namespace Insurance.Application.Interfaces.Services;

public interface IPolicyMaintenanceService
{
    Task ExpirePoliciesAsync();
}
