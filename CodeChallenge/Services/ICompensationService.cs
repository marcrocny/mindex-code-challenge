using CodeChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Task<Compensation> AddOrUpdate(string employeeId, CompensationModel compensationModel);
        Task<IList<Compensation>> ListByEmployeeId(string employeeId);
    }
}