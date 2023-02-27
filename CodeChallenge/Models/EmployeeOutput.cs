using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge.Models
{
    public class EmployeeOutput
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }

        /// <summary>
        /// It is unclear from the README doc what this list should contain. Falling back to ID field.
        /// </summary>
        public IEnumerable<string> DirectReports { get; set; }

        public static EmployeeOutput From(Employee employee)
        {
            var model = new EmployeeOutput()
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                Department = employee.Department,
            };
            if (employee.DirectReports != null && employee.DirectReports.Any())
                model.DirectReports = employee.DirectReports.Select(e => e.EmployeeId);

            return model;
        }
    }
}
