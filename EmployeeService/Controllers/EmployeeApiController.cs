using EmployeeApi.Interfaces;
using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Converter;
using EmployeeBusiness.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Controllers
{

    [Produces("application/json")]
    public class EmployeeApiController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeApiController(IEmployeeService employeeService) 
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<Response> CreateEmployees()
        {
            List<BaseEmployee> baseEmployees = new List<BaseEmployee>();
            Stream stream = HttpContext.Request.Body;
            using (StreamReader reader = new StreamReader(stream))
            {
                string employees = await reader.ReadToEndAsync();
                JsonConverter[] converters = { new EmployeeConverter() };
                baseEmployees = JsonConvert.DeserializeObject<List<BaseEmployee>>(employees, new JsonSerializerSettings() { Converters = converters });
            }
            return await this._employeeService.CreateEmployees(baseEmployees);
        }

        [HttpPost]
        public async Task<Response> CreateEmployeeWithAttribute()
        {
            BaseEmployee baseEmployee;
            Stream stream = HttpContext.Request.Body;
            using (StreamReader reader = new StreamReader(stream))
            {
                string employee = await reader.ReadToEndAsync();
                JsonConverter[] converters = { new EmployeeConverter() };
                baseEmployee = JsonConvert.DeserializeObject<BaseEmployee>(employee, new JsonSerializerSettings() { Converters = converters });
            }
            return this._employeeService.CreateEmployeeWithAttribute(baseEmployee);
        }

        public async Task<Response> GetEmployeeForName(string firstName, string lastName, string midleName)
        {
            return await this._employeeService.GetEmployeeForName(firstName, lastName, midleName);
        }

        public async Task<Response> GetEmployeesWithSalariesBelowTheSpecifiedLevel(double salaries)
        {
            return await this._employeeService.GetEmployeesWithSalariesBelowTheSpecifiedLevel(salaries);
        }

        public async Task<Response> GetTotalMonthlySalaryOfTheFiveHighestPaidEmployees()
        {
            return await this._employeeService.GetTotalMonthlySalaryOfTheFiveHighestPaidEmployees();
        }
    }
}
