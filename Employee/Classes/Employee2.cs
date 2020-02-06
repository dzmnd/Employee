using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Attributes;
using System;

namespace EmployeeBusiness.Classes
{
    public class Employee2 : BaseEmployee
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public override string FirstName { get; set; } = "";
        public override string LastName { get; set; } = "";
        public override string MidleName { get; set; } = "";
        public override string TypeEmployee { get; set; } = "Employee2";
        public override double AverageSalary { get => PayRoll(); }
        [HourlySalary]
        public double HourlySalaryRate { get; set; }

        public override double PayRoll()
        {
            return 0.8 * 8 * HourlySalaryRate;
        }
    }
}
