using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbcontext = new SoftUniContext();

            //Exercise 03. Employees Full Information
            //string employeesFullInfo = GetEmployeesFullInformation(dbcontext);
            //Console.WriteLine(employeesFullInfo);

            //Exercise 04. Employees with Salary Over 50 000
            //string employeesWithSalaryOver50000 = GetEmployeesWithSalaryOver50000(dbcontext);
            //Console.WriteLine(employeesWithSalaryOver50000);

            //Exercise 05. Employees from Research and Development
            //string employeesFromRnD = GetEmployeesFromResearchAndDevelopment(dbcontext);
            //Console.WriteLine(employeesFromRnD);

            //Exercise 06. Adding a New Address and Updating Employee
            //string addressList = AddNewAddressToEmployee(dbcontext);
            //Console.WriteLine(addressList);

            //Exercise 07
            //string firstTenEmployeesWithProjectsBetween2001and2003 = GetEmployeesInPeriod(dbcontext);
            //Console.WriteLine(firstTenEmployeesWithProjectsBetween2001and2003);

            //Exercise 08
            //string getAddressByTown = GetAddressesByTown(dbcontext);
            //Console.WriteLine(getAddressByTown);

            //Exercise 09
            //string employee147 = GetEmployee147(dbcontext);
            //Console.WriteLine(employee147);

            //Exercise 10
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbcontext));

            //Exercise 11
            //Console.WriteLine(GetLatestProjects(dbcontext));

            //Exercise 12
            //Console.WriteLine(IncreaseSalaries(dbcontext));

            //Exercise 13
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbcontext));

            //Exercise 14
            //Console.WriteLine(DeleteProjectById(dbcontext));

            //Exercise 15
            Console.WriteLine(RemoveTown(dbcontext));

        }
        public static string RemoveTown(SoftUniContext context)
        {

            var employees = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList();

            employees.ForEach(e => e.AddressId = null);

            var result = context.Addresses.Where(a => a.Town.Name == "Seattle").ToList();

            context.Addresses.RemoveRange(result);

            var deletedTown = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

            context.Towns.Remove(deletedTown);

            context.SaveChanges();

            return $"{result.Count} addresses in Seattle were deleted";
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectToDelete = context.Projects.Find(2);

            var employees = context.Employees
                .Where(e => e.EmployeesProjects.FirstOrDefault(pr => pr.ProjectId == 2) != null)
                .ToList();

            foreach (var e in employees)
            {
                var project = e.EmployeesProjects.FirstOrDefault(p => p.ProjectId == 2);
                e.EmployeesProjects.Remove(project);
            }

            var ep = context.EmployeesProjects.Where(p => p.ProjectId == 2).ToList();

            context.EmployeesProjects.RemoveRange(ep);

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            var result = context.Projects.Take(10).Select(p => p.Name).ToList();

            return string.Join(Environment.NewLine, result);
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            foreach (var e in result.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                })
                .ToList();

            foreach (var employee in result.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary * 1.12m:f2})");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                    p.Description
                })
                .ToList();

            foreach (var project in result.OrderBy(p => p.Name))
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Departments
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    DepartmentManager = $"{d.Manager.FirstName} {d.Manager.LastName}",
                    DepartmentEmployees = d.Employees
                })
                .ToList();

            foreach (var d in result)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.DepartmentManager}");
                foreach (var e in d.DepartmentEmployees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeNames = $"{e.FirstName} {e.LastName}",
                    EmployeeJobTitle = e.JobTitle,
                    Projects = context.Projects
                        .OrderBy(p => p.Name)
                        .Where(pr => e.EmployeesProjects.FirstOrDefault(ep => ep.ProjectId == pr.ProjectId) != null)
                        .Select(pr => new
                        {
                            ProjectName = pr.Name
                        })
                        .ToList(),

                })
                .FirstOrDefault();

            sb.AppendLine($"{employee147.EmployeeNames} - {employee147.EmployeeJobTitle}");
            foreach (var p in employee147.Projects)
            {
                sb.AppendLine($"{p.ProjectName}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Addresses
                .AsNoTracking()
                .OrderByDescending(e => e.Employees.Count)
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count,
                })
                .ToList();

            foreach (var address in result)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext dbcontext)
        {
            StringBuilder sb = new StringBuilder();

            var employees = dbcontext.Employees
                .Take(10)
                .Select(e => new
                {
                    EmployeeNames = $"{e.FirstName} {e.LastName}",
                    ManagerNames = $"{e.Manager.FirstName} {e.Manager.LastName}",
                    Projects = e.EmployeesProjects
                        .Where(ep =>
                            ep.Project.StartDate.Year >= 2001 &&
                            ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStart = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                        ProjectEnd = ep.Project.EndDate.HasValue
                            ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                            : "not finished",
                    })
                });


            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeNames} - Manager: {e.ManagerNames}");
                if (e.Projects.Any())
                {
                    foreach (var p in e.Projects)
                    {
                        sb.AppendLine($"--{p.ProjectName} - {p.ProjectStart} - {p.ProjectEnd}");
                    }
                }
            }

            return sb.ToString().Trim();
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    firstName = e.FirstName,
                    middleName = e.MiddleName,
                    lastName = e.LastName,
                    jobTitle = e.JobTitle,
                    salary = e.Salary,
                })
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.firstName} {e.lastName} {e.middleName} {e.jobTitle} {e.salary:f2}"));

            return sb.ToString().Trim();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    firstName = e.FirstName,
                    salary = e.Salary,
                })
                .Where(e => e.salary > 50000)
                .OrderBy(e => e.firstName)
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.firstName} - {e.salary:f2}"));

            return sb.ToString().Trim();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary,
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:f2}"));

            return sb.ToString().Trim();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            employee!.Address = newAddress;

            context.SaveChanges();

            var take10EmployeeAddresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToList();

            take10EmployeeAddresses.ForEach(a => sb.AppendLine($"{a}"));

            return sb.ToString().Trim();
        }
    }
}
