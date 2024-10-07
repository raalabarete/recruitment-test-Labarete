using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase
    {
        public ListController()
        {
        }

        [HttpGet]
        public List<Employee> getList() {
            Regex rg = new Regex(@"^[d-zD-Z]");
            var employees = new List<Employee>();
            var resEmployees = new List<Employee>();
            int sumValue = 0;
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            try
            {
                using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();

                    var queryCmd = connection.CreateCommand();
                    queryCmd.CommandText = @"SELECT Name, Value FROM Employees";
                    using (var reader = queryCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                Name = reader.GetString(0),
                                Value = reader.GetInt32(1)
                            });
                        }
                    }
                }
                List<string> empName = new List<string>();
                foreach (Employee employee in employees)
                {
                    if (!rg.IsMatch(employee.Name))
                    {
                        resEmployees.Add(new Employee
                        {
                            Name = employee.Name,
                            Value = employee.Value
                        });
                        sumValue += employee.Value;
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (sumValue >= 11171)
                return resEmployees;
            else
                return null;
        }
    }
}
