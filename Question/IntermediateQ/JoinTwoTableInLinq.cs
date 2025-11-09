namespace dotnetCoreInterviewPrepDemo.Question.IntermediateQ;

public class JoinTwoTableInLinq
{
    public static void JoinTowTablesInLinq()
    {
        // Sample data
        var departments = new List<Department>
        {
            new Department { deptId = 1, deptName = "IT" },
            new Department { deptId = 2, deptName = "HR" },
            new Department { deptId = 3, deptName = "Finance" }
        };

        var employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John", deptId = 1 },
            new Employee { Id = 2, Name = "Jane", deptId = 1 },
            new Employee { Id = 3, Name = "Bob", deptId = 2 },
            new Employee { Id = 4, Name = "Alice", deptId = 3 },
            new Employee { Id = 5, Name = "Charlie", deptId = 0 } // No department
        };
        
        
        // inner join -Employee with their departments
        var employeesWithDept = from e in employees
            join d in departments on e.deptId equals d.deptId
            select new
            {
                EmployeeName = e.Name,
                Department = d.deptName
            };
        
        Console.WriteLine("Employees with Departments:");
        foreach (var item in employeesWithDept)
        {
            Console.WriteLine($"{item.EmployeeName} - {item.Department}");
        }
    }
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int deptId { get; set; }
}

public class Department
{
    public int deptId { get; set; }
    public string deptName { get; set; }
}