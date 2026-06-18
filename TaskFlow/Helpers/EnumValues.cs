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
}
