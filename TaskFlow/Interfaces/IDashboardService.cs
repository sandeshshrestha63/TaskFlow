using TaskFlow.ViewModels.Dashboard;

namespace TaskFlow.Interfaces
{
    public interface IDashboardService
    {
        Task<EmployeeDashboardVM> GetEmployeeDashboardAsync(int companyId, int employeeId);
        Task<CompanyDashboardVM> GetCompanyDashboardAsync(int companyId);
        Task<SuperAdminDashboardVM> GetSuperAdminDashboardAsync();

    }
}
