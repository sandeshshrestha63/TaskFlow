using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.SeedData
{
    public class InitialSeeder
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await SeedProjectStatusesAsync(context);
            await SeedProjectPrioritiesAsync(context);
            await SeedProjectRolesAsync(context);
        }

        private static async Task SeedProjectStatusesAsync(AppDbContext context)
        {
            if (await context.ProjectStatus.AnyAsync())
                return;

            context.ProjectStatus.AddRange(
                new ProjectStatus { Name = "Planning", IsDefault = true, IsActive = true, DisplayOrder = 1 },
                new ProjectStatus { Name = "Active", IsDefault = false, IsActive = true, DisplayOrder = 2 },
                new ProjectStatus { Name = "On Hold", IsDefault = false, IsActive = true, DisplayOrder = 3 },
                new ProjectStatus { Name = "Completed", IsDefault = false, IsActive = true, DisplayOrder = 4 },
                new ProjectStatus { Name = "Cancelled", IsDefault = false, IsActive = true, DisplayOrder = 5 }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedProjectPrioritiesAsync(AppDbContext context)
        {
            if (await context.ProjectPriorities.AnyAsync())
                return;

            context.ProjectPriorities.AddRange(
                new ProjectPriority
                {
                    Name = "Low",
                    IsActive = true,
                    DisplayOrder = 1
                },
                new ProjectPriority
                {
                    Name = "Medium",
                    IsActive = true,
                    DisplayOrder = 2
                },
                new ProjectPriority
                {
                    Name = "High",
                    IsActive = true,
                    DisplayOrder = 3
                },
                new ProjectPriority
                {
                    Name = "Critical",
                    IsActive = true,
                    DisplayOrder = 4
                }
            );

            await context.SaveChangesAsync();
        }
        private static async Task SeedProjectRolesAsync(AppDbContext context)
        {
            if (await context.ProjectRoles.AnyAsync())
                return;

            var roles = new List<ProjectRole>
            {
                new()
                {
                    Name = "Project Manager",
                    Description = "Responsible for overall project planning and delivery.",
                    DisplayOrder = 1,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "Team Lead",
                    Description = "Leads a development team within the project.",
                    DisplayOrder = 2,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "Developer",
                    Description = "Develops and maintains application features.",
                    DisplayOrder = 3,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "QA Engineer",
                    Description = "Tests and validates software quality.",
                    DisplayOrder = 4,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "UI/UX Designer",
                    Description = "Designs user interfaces and user experiences.",
                    DisplayOrder = 5,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "Business Analyst",
                    Description = "Analyzes business requirements and processes.",
                    DisplayOrder = 6,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "Product Owner",
                    Description = "Defines product vision and prioritizes the backlog.",
                    DisplayOrder = 7,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "Scrum Master",
                    Description = "Project visibility without day-to-day execution.",
                    DisplayOrder = 8,
                    IsSystem = true,
                    IsActive = true
                },
                new()
                {
                    Name = "StakeHolder",
                    Description = "Facilitates Agile ceremonies and removes impediments.",
                    DisplayOrder = 8,
                    IsSystem = true,
                    IsActive = true
                }
            };

            await context.ProjectRoles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
