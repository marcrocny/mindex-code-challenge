using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetById(string id);
        Task<Employee> Create(Employee employee);
        Task<Employee> Update(Employee newEmployeeInfo);
    }
}
