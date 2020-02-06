using EmployeeBusiness.AbstractClasses;
using EmployeeBusiness.Classes;
using EmployeeBusiness.Converter;
using EmployeeBusiness.Models;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClient
{
    class EmployeeClient
    {
        private static readonly List<string> commands = new List<string> { 
            "Create Employees", 
            "Get Employee For Name", 
            "Get Employees With Salaries Below The Specified Level",
            "Get Total Monthly Salary Of The Five Highest Paid Employees",
            "Create Employee With Attribute",
            "Exit" 
        };
        private static readonly string URL = "http://localhost:5000/EmployeeApi/";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Employee Client running...");
            await RunAsync();
        }

        private static async Task RunAsync()
        {
            bool isRunning = true;
            string commandsString = string.Join("\n", commands);
            while (isRunning)
            {
                Console.WriteLine("Please enter your command from list:");
                Console.WriteLine($"{commandsString}\n");

                string command = Console.ReadLine();
                isRunning = command.ToLower() != "exit";
                if (!isRunning)
                {
                    break;
                }
                if (!commands.Select(c => c.ToLower()).Contains(command.ToLower()))
                {
                    Console.WriteLine($"Command {command} isn't valid command name \n");
                    continue;
                }
                switch (command.ToLower())
                {
                    case "get employee for name":
                        Dictionary<string, string> nameParameters = new Dictionary<string, string>();
                        Console.WriteLine("Enter First Name");
                        string firstName = Console.ReadLine();
                        nameParameters.Add("firstName", firstName);
                        Console.WriteLine("Enter Last Name");
                        string lastName = Console.ReadLine();
                        nameParameters.Add("lastName", lastName);
                        Console.WriteLine("Enter Midle Name");
                        string midleName = Console.ReadLine();
                        nameParameters.Add("midleName", midleName);

                        Console.WriteLine("Geting employee...\n");
                        Response result = await Get("GetEmployeeForName", nameParameters);
                        Console.WriteLine($"{result.Message}\n");
                        if (result.IsSuccess)
                        {
                            ResponseEmployee employee = result.ResponseEmployee;
                            Console.WriteLine($"Name: {employee.FirstName} {employee.LastName} {employee.MidleName}");
                            Console.WriteLine($"Average Salary: {employee.AverageSalary}\n\n");
                        }
                        break;
                    case "get employees with salaries below the specified level":
                        Dictionary<string, string> salariesParameters = new Dictionary<string, string>();
                        Console.WriteLine("Enter salaries");
                        string salaries = Console.ReadLine();
                        salariesParameters.Add("salaries", salaries);

                        Console.WriteLine("Geting employee...\n");
                        Response resultSalaries = await Get("GetEmployeesWithSalariesBelowTheSpecifiedLevel", salariesParameters);
                        Console.WriteLine($"{resultSalaries.Message}\n");

                        if (resultSalaries.IsSuccess)
                        {
                            List<ResponseEmployee> employees = resultSalaries.ResponseEmployees;
                            foreach (ResponseEmployee employee in employees)
                            {
                                Console.WriteLine($"Name: {employee.FirstName} {employee.LastName} {employee.MidleName}");
                                Console.WriteLine($"Average Salary: {employee.AverageSalary}\n\n");
                            }
                        }
                        break;
                    case "get total monthly salary of the five highest paid employees":
                        Dictionary<string, string> totalMonthlySalaryParameters = new Dictionary<string, string>();

                        Console.WriteLine("Geting total monthly salary of the five highest paid employees...\n");
                        Response totalMonthlySalaryResult = await Get("GetTotalMonthlySalaryOfTheFiveHighestPaidEmployees", totalMonthlySalaryParameters);
                        Console.WriteLine($"{totalMonthlySalaryResult.Message}\n");
                        if (totalMonthlySalaryResult.IsSuccess)
                        {
                            Console.WriteLine($"Total Monthly Salary Of The Five Highest Paid Employees: {totalMonthlySalaryResult.TotalMonthlySalary}\n\n");
                        }
                        break;
                    case "create employee with attribute":
                        Console.WriteLine("\nCreating employes with attribute...\n");
                        Response createEmployeeWithAttributeResult = await PostAsync("CreateEmployeeWithAttribute", false);
                        Console.WriteLine($"{createEmployeeWithAttributeResult.Message}\n");
                        if (createEmployeeWithAttributeResult.IsSuccess)
                        {
                            Console.WriteLine($"Average Salary: {createEmployeeWithAttributeResult.AverageSalary}\n\n");
                        }
                        break;
                    case "create employees":
                        Console.WriteLine("\nCreating employes...\n");
                        Response createResult = await PostAsync("CreateEmployees", true);
                        Console.WriteLine($"{createResult.Message}\n");
                        break;
                    default:
                        break;
                }
                continue;
            }
        }

        private static async Task<Response> Get(string path, Dictionary<string, string> parameters)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string fullUrl = WebUtilities.AddQueryString($"{URL}{path}", parameters);
                    HttpResponseMessage response = await client.GetAsync(fullUrl);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings {
                                                                  NullValueHandling = NullValueHandling.Include,
                                                                  MissingMemberHandling = MissingMemberHandling.Ignore
                                                              };
                    Response res = JsonConvert.DeserializeObject<Response>(result, settings);
                    return res;
                }
                catch (Exception ex)
                {
                    return new Response {
                                            IsSuccess = false,
                                            Message = $"Get error: {ex.Message}\n"
                                        };
                }
            }
        }

        private static async Task<Response> PostAsync(string path, bool isMultipleCreate)
        {
            List<BaseEmployee> employees = new List<BaseEmployee>();
            string employeesJson = string.Empty;
            if (isMultipleCreate)
            {
                #region Create New Employee
                Employee1 employee1 = new Employee1
                {
                    FirstName = "Jon",
                    LastName = "Doe",
                    TypeEmployee = "Employee1",
                    FixedMonthlySalary = 32
                };
                employees.Add(employee1);

                Employee1 employee2 = new Employee1
                {
                    FirstName = "Nik",
                    LastName = "Bak",
                    TypeEmployee = "Employee1",
                    FixedMonthlySalary = 12
                };
                employees.Add(employee2);

                Employee1 employee3 = new Employee1
                {
                    FirstName = "Cris",
                    LastName = "Mat",
                    MidleName = "Son",
                    TypeEmployee = "Employee1",
                    FixedMonthlySalary = 2
                };
                employees.Add(employee3);

                Employee2 employee4 = new Employee2
                {
                    FirstName = "Thrall",
                    LastName = "Orgim",
                    MidleName = "Dutotanovich",
                    TypeEmployee = "Employee2",
                    HourlySalaryRate = 22
                };
                employees.Add(employee4);

                Employee2 employee5 = new Employee2
                {
                    FirstName = "Artas",
                    LastName = "Menetil",
                    MidleName = "Nerzulovich",
                    TypeEmployee = "Employee2",
                    HourlySalaryRate = 11
                };
                employees.Add(employee5);

                Employee2 employee6 = new Employee2
                {
                    FirstName = "Durotan",
                    LastName = "Ogrim",
                    TypeEmployee = "Employee2",
                    HourlySalaryRate = 123
                };
                employees.Add(employee6);
                #endregion
                employeesJson = JsonConvert.SerializeObject(employees);
            }
            else
            {
                BaseEmployee Employee = (BaseEmployee)new Employee2
                {
                    FirstName = "Aroh",
                    LastName = "Arhot",
                    HourlySalaryRate = 17
                };
                employeesJson = JsonConvert.SerializeObject(Employee);
            }
            using (var client = new HttpClient { Timeout = TimeSpan.FromMinutes(2) })
            {
                try
                {
                    client.BaseAddress = new Uri(URL);
                    var buffer = Encoding.UTF8.GetBytes(employeesJson);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.PostAsync(path, byteContent);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings {
                                                                  NullValueHandling = NullValueHandling.Ignore,
                                                                  MissingMemberHandling = MissingMemberHandling.Ignore
                                                              };
                    Response res = JsonConvert.DeserializeObject<Response>(result, settings);
                    return res;
                }
                catch (Exception ex)
                {
                    return new Response {
                                            IsSuccess = false,
                                            Message = $"Post error: {ex.Message}\n"
                                        };
                }
            }
        }
    }
}
