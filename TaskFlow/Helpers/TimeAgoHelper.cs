namespace TaskFlow.Helpers
{
    public static class TimeAgoHelper
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            var utcDate = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            var timeSpan = DateTime.UtcNow - utcDate;

            if (timeSpan.TotalSeconds < 60)
                return "Just now";

            if (timeSpan.TotalMinutes < 60)
            {
                int minutes = (int)timeSpan.TotalMinutes;
                return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
            }

            if (timeSpan.TotalHours < 24)
            {
                int hours = (int)timeSpan.TotalHours;
                return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
            }

            if (timeSpan.TotalDays < 2)
                return "Yesterday";

            if (timeSpan.TotalDays < 7)
            {
                int days = (int)timeSpan.TotalDays;
                return $"{days} day{(days == 1 ? "" : "s")} ago";
            }

            if (timeSpan.TotalDays < 30)
            {
                int weeks = (int)(timeSpan.TotalDays / 7);
                return $"{weeks} week{(weeks == 1 ? "" : "s")} ago";
            }

            if (timeSpan.TotalDays < 365)
                return utcDate.ToLocalTime().ToString("dd MMM");

            return utcDate.ToLocalTime().ToString("dd MMM yyyy");
        }
    }
}