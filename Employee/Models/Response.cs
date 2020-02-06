using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeBusiness.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ResponseEmployee ResponseEmployee { get; set; }
        public List<ResponseEmployee> ResponseEmployees { get; set; }
        public double TotalMonthlySalary { get; set; }
        public double AverageSalary { get; set; }
    }

    public class ResponseEmployee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MidleName { get; set; } = "";
        public double AverageSalary { get; set; }
    }
}
