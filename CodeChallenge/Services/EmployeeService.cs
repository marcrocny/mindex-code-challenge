using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        /// <summary>
        /// Updates the employee with the matching employeeId.
        /// </summary>
        /// <remarks>Changed to a true update. Prior remove and replace was breaking the hierarchy.</remarks>
        public Employee Update(Employee newEmployeeInfo)
        {
            var loaded = _employeeRepository.GetById(newEmployeeInfo.EmployeeId);
            if (loaded == null) return null;

            // map onto loaded
            loaded.FirstName = newEmployeeInfo.FirstName;
            loaded.LastName = newEmployeeInfo.LastName;
            loaded.Position = newEmployeeInfo.Position;
            loaded.Department = newEmployeeInfo.Department;
            // let's not mess for now: loaded.DirectReports = ...

            _employeeRepository.SaveAsync().Wait();

            return loaded;
        }
    }
}
