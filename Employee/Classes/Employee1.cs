using EmployeeBusiness.AbstractClasses;
using System;

namespace EmployeeBusiness.Classes
{
    public class Employee1 : BaseEmployee
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public override string FirstName { get; set; } = "";
        public override string LastName { get; set; } = "";
        public override string MidleName { get; set; } = "";
        public override string TypeEmployee { get; set; } = "Employee1";
        public override double AverageSalary { get => PayRoll(); }
        public double FixedMonthlySalary { get; set; }

        public override double PayRoll()
        {
            return this.FixedMonthlySalary;
        }
    }
}
