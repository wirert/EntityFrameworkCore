using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();

        Console.WriteLine(RemoveTown(context));
    }

    //03. Employees Full Information
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .AsNoTracking()
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            });

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //04. Employees with Salary Over 50 000
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees.AsNoTracking()
            .Where(e => e.Salary > 50000)
            .Select(e => new { e.FirstName, e.Salary })
            .OrderBy(e => e.FirstName);

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //05. Employees from Research and Development
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .AsNoTracking()
            .Where(e => e.Department.Name == "Research and Development")
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            });

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //06. Adding a New Address and Updating Employee
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        context.Employees.FirstOrDefault(e => e.LastName == "Nakov")!.Address = newAddress;
        context.SaveChanges();

        var employeesAddresses = context.Employees
                                    .AsNoTracking()
                                    .OrderByDescending(e => e.AddressId)
                                    .Take(10)
                                    .Select(e => e.Address!.AddressText)
                                    .ToArray();

        return string.Join(Environment.NewLine, employeesAddresses);
    }

    //07. Employees and Projects
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
                        .AsNoTracking()
                        .Take(10)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            ManagerFirstName = e.Manager!.FirstName,
                            ManagerLastName = e.Manager!.LastName,
                            Projects = e.EmployeesProjects
                                        .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                                    ep.Project.StartDate.Year <= 2003)
                                        .Select(p => new
                                        {
                                            p.Project.Name,
                                            StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                            EndDate = p.Project.EndDate == null ? "not finished"
                                                                      : p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                        })
                                        .ToArray()
                        })
                        .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

            foreach (var p in e.Projects)
            {
                sb.AppendLine($"--{p.Name} - {p.StartDate} - {p.EndDate}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    //08. Addresses by Town
    public static string GetAddressesByTown(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var addresses = context.Addresses
                        .AsNoTracking()
                        .OrderByDescending(a => a.Employees.Count)
                        .ThenBy(a => a.Town!.Name)
                        .ThenBy(a => a.AddressText)
                        .Take(10)
                        .Select(a => new
                        {
                            a.AddressText,
                            TownName = a.Town!.Name,
                            EmployeeCount = a.Employees.Count
                        });

        foreach (var a in addresses)
        {
            sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
        }

        return sb.ToString().TrimEnd();
    }

    //09. Employee 147
    public static string GetEmployee147(SoftUniContext context)
    {
        var employee = context.Employees
            .AsNoTracking()
            .Where(e => e.EmployeeId == 147)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                Projects = e.EmployeesProjects
                            .Select(p => new { p.Project.Name })
                            .OrderBy(p => p.Name)
                            .ToArray()
            })
            .FirstOrDefault();

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

        foreach (var p in employee.Projects)
        {
            sb.AppendLine(p.Name);
        }

        return sb.ToString().TrimEnd();
    }

    //10. Departments with More Than 5 Employees
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var departments = context.Departments
            .AsNoTracking()
            .Where(d => d.Employees.Count > 5)
            .OrderBy(d => d.Employees.Count)
            .ThenBy(d => d.Name)
            .Select(d => new
            {
                d.Name,
                d.Manager.FirstName,
                d.Manager.LastName,
                Employees = d.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle
                })
                .ToArray()
            });

        StringBuilder sb = new StringBuilder();

        foreach (var d in departments)
        {
            sb.AppendLine($"{d.Name} – {d.FirstName} {d.LastName}");

            foreach (var e in d.Employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    //11. Find Latest 10 Projects
    public static string GetLatestProjects(SoftUniContext context)
    {
        var projects = context.Projects
                        .AsNoTracking()
                        .OrderByDescending(p => p.StartDate)
                        .Take(10)
                        .OrderBy(p => p.Name)
                        .Select(p => new
                        {
                            p.Name,
                            p.Description,
                            StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        });

        StringBuilder sb = new StringBuilder();

        foreach (var p in projects)
        {
            sb.AppendLine(p.Name)
                .AppendLine(p.Description)
                .AppendLine(p.StartDate);
        }

        return sb.ToString().TrimEnd();
    }

    //12. Increase Salaries
    public static string IncreaseSalaries(SoftUniContext context)
    {
        string[] departments = new[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

        var emplForIncreese = context.Employees
            .Where(e => departments.Contains(e.Department.Name));

        foreach (var e in emplForIncreese)
        {
            e.Salary *= 1.12m;
        }

        context.SaveChanges();

        var employees = context.Employees
            .AsNoTracking()
            .Where(e => departments.Contains(e.Department.Name))
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            })
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName);

        StringBuilder sb = new StringBuilder();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
        }

        return sb.ToString().TrimEnd();
    }

    //13. Find Employees by First Name Starting With Sa
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var employees = context.Employees
            .AsNoTracking()
            .Where(e => e.FirstName.ToLower().StartsWith("sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})")
            .ToArray();

        return string.Join(Environment.NewLine, employees);
    }

    //14. Delete Project by Id
    public static string DeleteProjectById(SoftUniContext context)
    {
        context.EmployeesProjects.RemoveRange(context.EmployeesProjects.Where(p => p.ProjectId == 2));
        context.Projects.Remove(context.Projects.Find(2)!);
        context.SaveChanges();

        var projects = context.Projects.Take(10).Select(p => p.Name);

        return string.Join(Environment.NewLine, projects);
    }

    //15. Remove Town
    public static string RemoveTown(SoftUniContext context)
    {
        var addressessToRemove = context.Addresses.Where(a => a.Town.Name == "Seattle");
        int count = addressessToRemove.Count();

        foreach(var e in context.Employees.Where(e => e.Address.Town.Name == "Seattle"))
        {
            e.AddressId = null;
        }

        context.Addresses.RemoveRange(addressessToRemove);

        context.Towns.Remove(context.Towns.Where(t => t.Name == "Seattle").FirstOrDefault()!);

        context.SaveChanges();

        return $"{count} addresses in Seattle were deleted";
    }
}