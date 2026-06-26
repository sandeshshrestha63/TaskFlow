using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class TaskActivityService : ITaskActivityService
    {
        private readonly AppDbContext _db;
        public TaskActivityService(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddActivityAsync(int taskId, int employeeId, string activityType, string description)
        {
            var activity = new TaskActivity
            {
                EmployeeTaskId = taskId,
                EmployeeId = employeeId,
                ActivityType = activityType,
                Description = description,
                CreatedDate = DateTime.UtcNow
            };

            _db.TaskActivities.Add(activity);

            await _db.SaveChangesAsync();
        }
    }
}
