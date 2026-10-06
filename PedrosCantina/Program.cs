using PedrosCantina;

EmployeeRepository employeeRepository = new EmployeeRepository();
ScheduleRepository scheduleRepository = new ScheduleRepository();


// ========================================
// EMPLOYEE CRUD
// ========================================

Console.WriteLine("========================================");
Console.WriteLine("EMPLOYEE CRUD");
Console.WriteLine("========================================");


// READ ALL
Console.WriteLine("\n--- Get all employees ---");

List<Employee> employees = employeeRepository.GetAll();

foreach (Employee employee in employees)
{
    Console.WriteLine(
        $"{employee.EmployeeId} - " +
        $"{employee.FirstName} {employee.LastName} - " +
        $"{employee.Email}");
}


// READ BY ID
Console.WriteLine("\n--- Get employee by ID ---");

Employee employeeById = employeeRepository.GetById(2);

if (employeeById != null)
{
    Console.WriteLine(
        $"{employeeById.EmployeeId} - " +
        $"{employeeById.FirstName} {employeeById.LastName}");
}


// CREATE
Console.WriteLine("\n--- Create employee ---");

Employee testEmployee = new Employee
{
    FirstName = "Test",
    LastName = "Employee",
    Phone = "77777777",
    Email = "test@cantina.dk",
    CanLead = false
};

employeeRepository.Create(testEmployee);

Console.WriteLine("Employee created.");


// Find the employee we just created
int testEmployeeId = 0;

employees = employeeRepository.GetAll();

foreach (Employee employee in employees)
{
    if (employee.Email == "test@cantina.dk")
    {
        testEmployeeId = employee.EmployeeId;
    }
}


// UPDATE
if (testEmployeeId != 0)
{
    Console.WriteLine("\n--- Update employee ---");

    Employee employeeToUpdate =
        employeeRepository.GetById(testEmployeeId);

    employeeToUpdate.FirstName = "Updated";
    employeeToUpdate.Phone = "88888888";
    employeeToUpdate.Email = "updated@cantina.dk";

    employeeRepository.Update(employeeToUpdate);

    Console.WriteLine("Employee updated.");


    // DELETE
    Console.WriteLine("\n--- Delete employee ---");

    employeeRepository.Delete(testEmployeeId);

    Console.WriteLine("Employee deleted.");
}


// ========================================
// DATABASE QUERIES
// ========================================

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("MONTHLY SCHEDULE");
Console.WriteLine("========================================");

scheduleRepository.ShowMonthlySchedule(2026, 10);


Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("MONTHLY EMPLOYEE WORKLOAD");
Console.WriteLine("========================================");

scheduleRepository.ShowMonthlyWorkload(2026, 10);


Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("AVAILABLE EMPLOYEES FOR SHIFT 1");
Console.WriteLine("========================================");

scheduleRepository.ShowAvailableEmployees(1);


Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("YEARLY EMPLOYEE WORKLOAD");
Console.WriteLine("========================================");

scheduleRepository.ShowYearlyWorkload(2026);


Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("PROGRAM FINISHED");
Console.WriteLine("========================================");