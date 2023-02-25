// See https://aka.ms/new-console-template for more information


using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using System.Text;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();

        Console.WriteLine(GetEmployeesFullInformation(context));
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb= new StringBuilder();

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
}