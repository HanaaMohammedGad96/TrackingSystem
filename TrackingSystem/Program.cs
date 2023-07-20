// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using TrackingSystem;

using (var context = new ApplicationDbContext())
{
    var result = from e in context.Employees
                 join m in context.Employees on e.ManagerId equals m.Id
                 join ep in context.EmployeeProjects on e.Id equals ep.EmployeeId
                 join p in context.Projects on ep.ProjectId equals p.Id
                 group ep by new { MN=m.Name, PN=p.Name } into g
                 select new
                 {
                     ManagerName = g.Key.MN,
                     ProjectName = g.Key.PN,
                     TotalTime = g.Sum(ep => ep.TimeInMinutes)
                 };

    Console.WriteLine("\nStored Procedure: Get managerName and total time >= 1000 for its employees per project according startDate and endDate");
    foreach (var item in result)
    {
        Console.WriteLine($"{item.ManagerName} - {item.ProjectName}: {item.TotalTime} minutes");
    }

}
Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
using (var context = new ApplicationDbContext())
{
    var startDate = new DateTime(2023, 07, 01);
    var endDate = new DateTime(2023, 07, 31);
    var threshold = 1000;

    var managerProjectTimeQuery = context.Employees
        .Join(context.Employees, e => e.ManagerId, m => m.Id, (e, m) => new { e, m })
        .Join(context.EmployeeProjects, em => em.e.Id, ep => ep.EmployeeId, (em, ep) => new { em, ep })
        .Join(context.Projects, emp => emp.ep.ProjectId, p => p.Id, (emp, p) => new { emp.em.m, p, emp.ep })
        .Where(result => result.ep.WorkDate >= startDate && result.ep.WorkDate <= endDate)
        .GroupBy(result => new { MN=result.m.Name, PN=result.p.Name })
        .Where(g => g.Sum(x => x.ep.TimeInMinutes) >= threshold)
        .Select(g => new
        {
            ManagerName = g.Key.MN,
            ProjectName = g.Key.PN,
            TotalTime = g.Sum(x => x.ep.TimeInMinutes)
        });

    // Display the results
    Console.WriteLine("Query: Get managerName and total time >= 1000 for its employees per project according startDate and endDate");
    foreach (var result in managerProjectTimeQuery)
    {
        Console.WriteLine($"{result.ManagerName} - {result.ProjectName}: {result.TotalTime} minutes");
    }
}
