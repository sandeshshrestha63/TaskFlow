namespace TaskFlow.Constants
{
    public class Constants
    {
    }
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string CompanyAdmin = "CompanyAdmin";
        public const string Employee = "Employee";
    }
    public static class Policies
    {
        public const string SuperAdminOnly = "SuperAdminOnly";
        public const string CompanyAccess = "CompanyAccess";
        public const string EmployeeAccess = "EmployeeAccess";
    }
    public static class CustomClaims
    {
        public const string CompanyId = "CompanyId";
        public const string EmployeeId = "EmployeeId";
    }
    public static class TempDataKeys
    {
        public const string Success = "success";
        public const string Error = "error";
    }
}
