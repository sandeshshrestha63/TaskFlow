using TaskFlow.Interfaces;

namespace TaskFlow.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

    }
}
