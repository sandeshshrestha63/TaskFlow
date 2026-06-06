namespace TaskFlow.Interfaces
{
    public interface ICurrentUserServices
    {
        string? UserId { get; }
        int CompanyId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
