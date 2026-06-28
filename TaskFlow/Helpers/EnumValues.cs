namespace TaskFlow.Helpers
{
    public class EnumValues
    {
    }
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        OnHold = 3,
        Completed = 4,
        Cancelled = 5
    }
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
    public enum NotificationType
    {
        TaskAssigned = 1,
        TaskUpdated = 2,
        TaskCompleted = 3,

        CommentAdded = 10,

        ProjectAssigned = 20,
        ProjectCompleted = 21,

        EmployeeInvited = 30,

        Mention = 40,

        DeadlineReminder = 50,

        System = 100
    }
    public enum ReferenceType
    {
        Task = 1,
        Project = 2,
        Comment = 3,
        Employee = 4,
        Department = 5
    }
    public enum NotificationPriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Critical = 4
    }
}
