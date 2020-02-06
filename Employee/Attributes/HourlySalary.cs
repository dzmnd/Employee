using System;

namespace EmployeeBusiness.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HourlySalary : Attribute
    {
        public double HourlySalaryRate { get; set; }
    }
}
