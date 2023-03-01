using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly EmployeeContext employeeContext;

        public CompensationService(EmployeeContext employeeContext)
        {
            this.employeeContext = employeeContext;
        }

        /// <summary>Adds compensation, or updates the compensation if the date matches an existing.</summary>
        /// <returns>If the employeeId exists, the new data model; else `null`.</returns>
        public async Task<Compensation> AddOrUpdate(string employeeId, CompensationModel compensationModel)
        {
            var employeeExists = await employeeContext.Employees.AnyAsync(e => e.EmployeeId == employeeId);
            if (!employeeExists) return null;

            // create and populate a new data model to get the date-flattening from `Populate()`
            var compensation = new Compensation { EmployeeId = employeeId };
            compensationModel.Populate(compensation);

            var existing = await employeeContext.Compensation.FindAsync(employeeId, compensation.EffectiveDate);

            if (existing == null)
                employeeContext.Compensation.Add(compensation);
            else
                compensationModel.Populate(existing);

            await employeeContext.SaveChangesAsync();
            return compensation;
        }

        /// <summary>
        /// Gets the full compensation list for the given employeeId.
        /// </summary>
        /// <returns>If the employeeId exists, all stored <see cref="Compensation"/> (or an empty list); else `null`.</returns>
        public async Task<IList<Compensation>> ListByEmployeeId(string employeeId)
        {
            var employeeExists = await employeeContext.Employees.AnyAsync(e => e.EmployeeId == employeeId);
            if (!employeeExists) return null;

            return await employeeContext.Compensation.Where(c => c.EmployeeId == employeeId).ToListAsync();
        }
    }
}
