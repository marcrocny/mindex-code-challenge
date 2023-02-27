using CodeChallenge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeDataSeeder
    {
        private readonly EmployeeContext _employeeContext;
        private const string EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";

        public EmployeeDataSeeder(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public async Task Seed()
        {
            if(!_employeeContext.Employees.Any())
            {
                List<Employee> employees = LoadEmployees();
                _employeeContext.Employees.AddRange(employees);

                await _employeeContext.SaveChangesAsync();
            }
        }

        private static List<Employee> LoadEmployees()
        {
            using var fs = new FileStream(EMPLOYEE_SEED_DATA_FILE, FileMode.Open);
            using var sr = new StreamReader(fs);
            using var jr = new JsonTextReader(sr);

            var serializer = new JsonSerializer();

            List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);

            return employees;
        }
    }
}
