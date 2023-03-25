// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectImportDto[]), new XmlRootAttribute("Projects"));
            var importDtos = (ProjectImportDto[])serializer.Deserialize(new StringReader(xmlString))!;
            var sb = new StringBuilder();
            var projects = new HashSet<Project>();

            foreach (var pDto in importDtos)
            {
                var isdueDate = IsValidDate(pDto.DueDate, out var parsedDueDate);

                DateTime? dueDate = string.IsNullOrWhiteSpace(pDto.DueDate)
                     ? null
                     : parsedDueDate;

                if (!IsValid(pDto) 
                    || !IsValidDate(pDto.OpenDate, out var openDate)
                    || (!string.IsNullOrWhiteSpace(pDto.DueDate) && !isdueDate)
                    )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                var project = new Project()
                {
                    Name = pDto.Name,
                    OpenDate = openDate,
                    DueDate = dueDate
                };

                var tasks = new HashSet<Task>();

                foreach (var tDto in pDto.Tasks)
                {
                    if (tasks.Any(t => t.Name == tDto.Name))
                    {
                        continue;
                    }

                    var isValidTaskDueDate = IsValidDate(tDto.DueDate, out var taskDueDate);

                    if (!IsValid(tDto) 
                        || !IsValidDate(tDto.OpenDate, out var taskOpenDate) 
                        || !isValidTaskDueDate
                        || taskOpenDate > taskDueDate
                        //|| (project.DueDate != null && taskDueDate > project.DueDate)
                        )                        
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    tasks.Add(new Task()
                    {
                        Name = tDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)tDto.ExecutionType,
                        LabelType = (LabelType)tDto.LabelType,
                        Project = project
                    });                    
                }
                project.Tasks = tasks;
                projects.Add(project);

                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeeDtos = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString);
            var employees = new HashSet<Employee>();
            var sb = new StringBuilder();
           

            foreach (var eDto in employeeDtos)
            {
                if (!IsValid(eDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee()
                {
                    Username = eDto.Username,
                    Email = eDto.Email,
                    Phone = eDto.Phone
                };

                var taskIds = context.Tasks.Select(t => t.Id).ToArray();
                var tasks = new HashSet<EmployeeTask>();

                foreach (var taskId in eDto.Tasks.Distinct())
                {
                    if (!taskIds.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    tasks.Add(new EmployeeTask()
                    {
                        TaskId = taskId,
                        Employee = employee
                    });
                }

                employee.EmployeesTasks = tasks;
                employees.Add(employee);

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, tasks.Count));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static bool IsValidDate(string? input, out DateTime date)
                 => DateTime.TryParseExact
                             (input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }
}