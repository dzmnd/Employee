using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi.Interfaces
{
    public interface IEmployeeService
    {
        Task<Response> GetEmployeeForName(string firstName, string lastName, string midleName);
        Task<Response> GetEmployeesWithSalariesBelowTheSpecifiedLevel(double salaries);
        Task<Response> GetTotalMonthlySalaryOfTheFiveHighestPaidEmployees();
        Task<Response> CreateEmployees(List<BaseEmployee> employee);
        Response CreateEmployeeWithAttribute(BaseEmployee employee);
    }
}
