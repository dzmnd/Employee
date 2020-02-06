using System;

namespace EmployeeBusiness.AbstractClasses
{
    public abstract class BaseEmployee
    {
        public BaseEmployee() { }
        public abstract Guid Id { get; set; }
        public abstract string FirstName { get; set; }
        public abstract string LastName { get; set; }
        public abstract string MidleName { get; set; }
        public abstract double AverageSalary { get; }
        public abstract string TypeEmployee { get; set; }

        public abstract double PayRoll();
    }
}
