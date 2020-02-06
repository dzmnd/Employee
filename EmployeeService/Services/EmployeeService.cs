using EmployeeApi.Interfaces;
using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Attributes;
using EmployeeBusiness.Classes;
using EmployeeBusiness.Converter;
using EmployeeBusiness.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmployeeApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private List<BaseEmployee> employees = new List<BaseEmployee>();

        private static readonly string PATH = Path.GetFullPath(@"C:\Projects\Employee\EmployeeService\JsonFile\Employees.json");

        public EmployeeService() 
        {
            GetEmployeesAsync();
        }

        public async Task<Response> GetEmployeeForName(string firstName, string lastName, string midleName)
        {
            Response response = new Response();
            try
            {
                bool isSearchForFirstName = !string.IsNullOrWhiteSpace(firstName);
                bool isSearchForLastName = !string.IsNullOrWhiteSpace(lastName);
                bool isSearchForMidlestName = !string.IsNullOrWhiteSpace(midleName);

                List<BaseEmployee> _employees = employees.Where(e => (!isSearchForFirstName || e.FirstName.ToLower() == firstName.ToLower()) && 
                                                                     (!isSearchForLastName || e.LastName.ToLower() == lastName.ToLower()) && 
                                                                     (!isSearchForMidlestName || e.MidleName.ToLower() == midleName.ToLower())).ToList();

                if (_employees.Count() == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Get employee for name error: Employee doesn't exist";
                }
                if (_employees.Count() == 1)
                {
                    response.IsSuccess = true;
                    response.Message = "Get employee for name seccessfuly";
                    response.ResponseEmployee = _employees.Select(employee => { 
                                                                                 return new ResponseEmployee {
                                                                                                                 Id = employee.Id,
                                                                                                                 FirstName = employee.FirstName,
                                                                                                                 LastName = employee.LastName,
                                                                                                                 MidleName = employee.MidleName,
                                                                                                                 AverageSalary = employee.AverageSalary
                                                                                                             }; 
                                                                             }
                                                                 ).First();
                }
                if (_employees.Count() > 1)
                {
                    response.IsSuccess = false;
                    response.Message = "Get employee for name error: Employee name must be unique";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Get employee for name error: {ex.Message}\n";
            }
            return response;
        }

        public async Task<Response> GetEmployeesWithSalariesBelowTheSpecifiedLevel(double salaries)
        {

            Response response = new Response();
            try
            {
                List<BaseEmployee> _employees = employees.Where(e => e.AverageSalary < salaries).ToList();
                response.IsSuccess = true;
                response.Message = "Get employees for with salaries below the specified level seccessfuly";
                response.ResponseEmployees = _employees.Select(employee => {
                                                                               return new ResponseEmployee
                                                                               {
                                                                                   Id = employee.Id,
                                                                                   FirstName = employee.FirstName,
                                                                                   LastName = employee.LastName,
                                                                                   MidleName = employee.MidleName,
                                                                                   AverageSalary = employee.AverageSalary
                                                                               };
                                                                           }
                                                             ).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Get employees for with salaries below the specified level error: {ex.Message}";
            }
            return response;
        }

        public async Task<Response> GetTotalMonthlySalaryOfTheFiveHighestPaidEmployees()
        {
            Response response = new Response();
            try
            {
                double salary = employees.OrderByDescending(e => e.AverageSalary).Take(5).Sum(e => e.AverageSalary);
                response.IsSuccess = true;
                response.Message = "Get total monthly salary of the five highest paid employees seccessfuly";
                response.TotalMonthlySalary = salary;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Get total monthly salary of the five highest paid employees error: {ex.Message}";
            }
            return response;
        }

        public async Task<Response> CreateEmployees(List<BaseEmployee> baseEmployee)
        {
            employees = employees.Concat(baseEmployee).ToList();
            return SaveEmployes(baseEmployee);
        }

        public Response CreateEmployeeWithAttribute(BaseEmployee employee)
        {
            Response response = new Response();
            try
            {
                PropertyInfo employeePropertyWithAttribute = employee.GetType().GetProperties().Where(e => e.CustomAttributes.Any(a => a.AttributeType == typeof(HourlySalary))).First();
                employee.GetType().GetProperty(employeePropertyWithAttribute.Name).SetValue(employee, 300);

                bool isSuccess = SaveEmploye(employee);

                if (isSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = "Create employee with attribute seccessfuly";
                    response.AverageSalary = employee.AverageSalary;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Create employee with attribute error";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Create employee with attribute error: {ex.Message}";
            }
            return response;
        }

        private bool SaveEmploye(BaseEmployee employee)
        {
            try
            {
                employees.Add(employee);
                string employes = JsonConvert.SerializeObject(employees);
                using (StreamWriter sw = new StreamWriter(PATH, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(employes);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GetEmployeesAsync()
        {
            try
            {
                JsonConverter[] converters = { new EmployeeConverter() };
                using (StreamReader sr = new StreamReader(PATH))
                {
                    string _employees = sr.ReadToEnd();
                    employees = JsonConvert.DeserializeObject<List<BaseEmployee>>(_employees, new JsonSerializerSettings() { Converters = converters });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Response SaveEmployes(List<BaseEmployee> employees)
        {
            try
            {
                employees = this.employees.Concat(employees).ToList();
                string employes = JsonConvert.SerializeObject(employees);
                using (StreamWriter sw = new StreamWriter(PATH, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(employes);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Create employees seccessfuly"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Create employees error: {ex.Message}"
                };
            }
        }
    }
}
