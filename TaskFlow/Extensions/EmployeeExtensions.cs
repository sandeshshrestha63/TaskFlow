using TaskFlow.Models;

namespace TaskFlow.Extensions
{
    public static class EmployeeExtensions
    {
        public static string GetFullName(this Employee employee)
        {
            if (employee == null)
                return string.Empty;

            return $"{employee.FirstName} {employee.LastName}".Trim();
        }
    }
}