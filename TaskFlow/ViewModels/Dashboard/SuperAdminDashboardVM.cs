namespace TaskFlow.ViewModels.Dashboard
{
    public class SuperAdminDashboardVM
    {
        public int Companies { get; set; }

        public int Employees { get; set; }

        public int Tasks { get; set; }

        public int Attachments { get; set; }

        public List<CompanySummaryVM> LatestCompanies { get; set; } = new();
    }
    public class CompanySummaryVM
        {
            public int Id { get; set; }

            public string CompanyName { get; set; } = string.Empty;

            public DateTime CreatedDate { get; set; }
        }
}