using System;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        // private readonly ILogger<IEmployeeRepository> _logger;
        private readonly EmployeeContext _employeeContext;

        public EmployeeRespository(//ILogger<IEmployeeRepository> logger,
            EmployeeContext employeeContext)
        {
            //_logger = logger;
            _employeeContext = employeeContext;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Task<Employee> GetById(string id)
            => _employeeContext.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);

        public Task SaveAsync()
            => _employeeContext.SaveChangesAsync();

        public Employee Remove(Employee employee)
            => _employeeContext.Remove(employee).Entity;

        public Task<Employee> GetWithChildren(string id)
            => _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);
    }
}
