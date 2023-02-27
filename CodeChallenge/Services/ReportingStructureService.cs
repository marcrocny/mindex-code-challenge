using CodeChallenge.Extensions;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository employeeRepository;

        public ReportingStructureService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<ReportingStructure> Get(string employeeId)
        {
            if (employeeId == null) throw new ArgumentNullException(nameof(employeeId));

            var employee = await employeeRepository.GetWithChildren(employeeId);
            if (employee == null) return null;
            if (!employee.DirectReports.Safe().Any())
                return new ReportingStructure { Employee = employee };

            IEnumerable<Employee> levelMembers = employee.DirectReports;
            int count = levelMembers.Count();
            employee.DirectReports = null;
            while (levelMembers.Any())
            {
                levelMembers =
                    (await Task.WhenAll(levelMembers.Select(e => employeeRepository.GetWithChildren(e.EmployeeId))))
                    .SelectMany(e => e.DirectReports.Safe());
                count += levelMembers.Count();
            }

            return new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = count,
            };
        }
    }
}
