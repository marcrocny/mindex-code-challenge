using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IReportingStructureService
    {
        Task<ReportingStructure> Get(string employeeId);
    }
}