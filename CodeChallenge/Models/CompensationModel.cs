using System;
using System.Collections.Generic;

namespace CodeChallenge.Models
{
    public class CompensationModel
    {
        public decimal Salary { get; set; }

        public string EffectiveDate { get; set; }

        public static CompensationModel Map(Compensation compensation)
            => new()
            {
                Salary = compensation.Salary,
                EffectiveDate = compensation.EffectiveDate.ToString("yyyy-MM-dd"),
            };

        public IEnumerable<string> Validate()
        {
            if (Salary <= 0) yield return "salary must be greater than zero";
            if (EffectiveDate == null)
                yield return "effectiveDate is required";
            else if (!DateTime.TryParse(EffectiveDate, out _))
                yield return $"{EffectiveDate} is not a valid effectiveDate";
        }

        public void Populate(Compensation compensation)
        {
            compensation.Salary = Salary;
            compensation.EffectiveDate = DateTime.Parse(EffectiveDate).Date;
        }
    }
}
