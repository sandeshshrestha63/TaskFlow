namespace TaskFlow.Interfaces
{
    public interface ITaskActivityService
    {
        Task AddActivityAsync(int taskId, int employeeId, string activityType, string description);
    }
}
