using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();

        /// <summary>
        /// Returns the employee with direct reports.
        /// </summary>
        Task<Employee> GetWithChildren(string id);
    }
}