using Microsoft.EntityFrameworkCore;
using TrackingSystem.Entities;

namespace TrackingSystem;

public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<EmployeeProject> EmployeeProjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Set your SQL Server connection string here
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TrackingSysDb2;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }
}
