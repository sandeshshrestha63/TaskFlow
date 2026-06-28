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
        public const string FullName = "FullName";
    }
    public static class TempDataKeys
    {
        public const string Success = "success";
        public const string Error = "error";
    }

    public static class TaskActivityTypes
    {
        public const string Created = "Created";
        public const string Updated = "Updated";
        public const string Assigned = "Assigned";
        public const string PriorityChanged = "Priority Changed";
        public const string StatusChanged = "Status Changed";
        public const string CommentAdded = "Comment Added";
        public const string Completed = "Completed";
        public const string AttachmentUploaded = "Attachment Uploaded";
        public const string AttachmentDeleted = "Attachment Deleted";
    }
    public static class CustomSettings
    {
        public const string AttachmentSettings = "AttachmentSettings";
    }
    public static class ErrorMessages
    {
        public const string TaskNotFound = "Task not found.";

        public const string NoFilesSelected = "Please select at least one file.";

        public const string FileTypeNotAllowed = "The selected file type is not allowed.";

        public const string FileIsEmpty = "The selected file is empty.";
    }
    public static class DashboardFilters
    {
        public const string Active = "active";

        public const string CompletedToday = "completedtoday";

        public const string Overdue = "overdue";

        public const string DueToday = "duetoday";

        public const string Critical = "critical";
    }
}

