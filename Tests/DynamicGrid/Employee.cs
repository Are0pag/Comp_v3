using System.Collections.ObjectModel;

namespace Tests.DynamicGrid;

public class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; }
}

public class EmployeeViewModel
{
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeViewModel()
    {
        Employees = new ObservableCollection<Employee>
        {
            new Employee 
            { 
                Name = "Иван Петров", 
                Age = 30, 
                Salary = 50000, 
                Department = "IT" 
            },
            new Employee 
            { 
                Name = "Анна Смирнова", 
                Age = 28, 
                Salary = 55000, 
                Department = "Маркетинг" 
            }
        };
    }
}
