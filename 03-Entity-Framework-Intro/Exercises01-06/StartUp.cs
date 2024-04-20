using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        var context = new SoftUniContext();

        //Exercise 03. Employees Full Information
        //string employeesFullInfo = GetEmployeesFullInformation(context);

        //Console.WriteLine(employeesFullInfo);


        //Exercise 04. Employees with Salary Over 50 000
        //string employeesWithSalaryOver50000 = GetEmployeesWithSalaryOver50000(context);

        //Console.WriteLine(employeesWithSalaryOver50000);


        //Exercise 05. Employees from Research and Development
        //string employeesFromRnD = GetEmployeesFromResearchAndDevelopment(context);

        //Console.WriteLine(employeesFromRnD);


        //Exercise 06. Adding a New Address and Updating Employee
        string addressList = AddNewAddressToEmployee(context);

        Console.WriteLine(addressList);

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

