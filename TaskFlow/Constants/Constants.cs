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
}
