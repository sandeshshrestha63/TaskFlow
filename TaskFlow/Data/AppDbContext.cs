using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // This constructor receives database configuration from ASP.NET Core's Dependency Injection system.

            // DbContextOptions<AppDbContext> contains all database-related settings such as:
            // - Connection string (SQL Server, etc.)
            // - Database provider (UseSqlServer, SQLite, etc.)
            // - Lazy loading / tracking behavior
            // - Other EF Core configuration options

            // : base(options)
            // This passes the configuration to the base DbContext class (EF Core engine),
            // so it knows HOW and WHERE to connect to the database.

            // Without this, EF Core would not know:
            // - Which database to connect to
            // - How to build the database model
            // - How to execute queries
        }
        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }

        public DbSet<EmployeeTaskStatus> EmployeeTaskStatus { get; set; }

        public DbSet<EmployeeTaskPriority> TaskPriorities { get; set; }

        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskActivity> TaskActivities { get; set; }
        public DbSet<EmployeeTaskAttachment> EmployeeTaskAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x => x.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(x => x.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x => x.AssignedToEmployee)
                .WithMany()
                .HasForeignKey(x => x.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x => x.EmployeeTaskStatus)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.EmployeeTaskStatusId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(x => x.EmployeeTaskPriority)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.EmployeeTaskPriorityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskComment>()
                .HasOne(x => x.EmployeeTask)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.EmployeeTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskComment>()
                .HasOne(x => x.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(x => x.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeTask>()
                .Property(x => x.EstimatedHours)
                .HasPrecision(18, 2);

            modelBuilder.Entity<EmployeeTask>()
                .Property(x => x.ActualHours)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.ApplicationUser)
                .WithOne()
                .HasForeignKey<Employee>(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TaskActivity>()
                .HasOne(x => x.EmployeeTask)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.EmployeeTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskActivity>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EmployeeTaskAttachment>()
                .HasOne(x => x.EmployeeTask)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.EmployeeTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTaskAttachment>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
