using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        //private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(//ILogger<EmployeeService> logger, 
            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            //_logger = logger;
        }

        public async Task<Employee> Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                await _employeeRepository.SaveAsync();
            }

            return employee;
        }

        public async Task<Employee> GetById(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                return await _employeeRepository.GetById(id);
            }

            return null;
        }

        /// <summary>
        /// Updates the employee with the matching employeeId.
        /// </summary>
        /// <remarks>Changed to a true update. Prior remove and replace was breaking the hierarchy.</remarks>
        public async Task<Employee> Update(Employee newEmployeeInfo)
        {
            var loaded = await _employeeRepository.GetById(newEmployeeInfo.EmployeeId);
            if (loaded == null) return null;

            // map onto loaded
            loaded.FirstName = newEmployeeInfo.FirstName;
            loaded.LastName = newEmployeeInfo.LastName;
            loaded.Position = newEmployeeInfo.Position;
            loaded.Department = newEmployeeInfo.Department;
            // let's not mess for now: loaded.DirectReports = ...

            await _employeeRepository.SaveAsync();

            return loaded;
        }
    }
}
