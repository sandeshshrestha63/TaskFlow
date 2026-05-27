using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
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
    }
}
