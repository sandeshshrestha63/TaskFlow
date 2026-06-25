namespace TaskFlow.Interfaces
{
    public interface ICurrentUserServices
    {
        string? UserId { get; }
        int CompanyId { get; }
        int EmployeeId { get; }
        string? FullName { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
