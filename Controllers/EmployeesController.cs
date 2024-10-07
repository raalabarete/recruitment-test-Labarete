using InterviewTest.DTOs;
using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public List<Employee> Get()
        {
            var employees = new List<Employee>();
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            try {
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
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return employees;
        }

        [HttpPost]
        [Route("/addEmployee")]
        public ActionResult addEmployee([FromBody] SubmitEmployeeDTO employee)
        {
            if (ModelState.IsValid)
            {
                string employeeName = employee.Name;
                int employeeVal = employee.Value;
                switch(employeeName[0])
                {
                    case 'E':
                        employeeVal += 1;
                        break;
                    case 'G':
                        employeeVal += 10;
                        break;
                    default:
                        employeeVal += 100;
                        break;
                }
                var sql = "INSERT INTO Employees " +
                        "VALUES (@Name, @Value)";
                var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
                try { 
                    using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                    {
                        connection.Open();

                        var queryCmd = new SqliteCommand(sql, connection);
                        queryCmd.Parameters.AddWithValue("@Name", employeeName);
                        queryCmd.Parameters.AddWithValue("@Value", employeeVal);
                        var rowInserted = queryCmd.ExecuteNonQuery();

                        Console.WriteLine($"The employee '{employeeName}' has been created successfully.");

                    }
                }
                catch (SqliteException ex)
{
                    return BadRequest(ex);
                }
            }
            return Ok("Employee " + employee.Name + "has been added");
        }



        [HttpPost]
        [Route("/updateEmployee")]
        public ActionResult updateEmployee([FromBody] SubmitEmployeeDTO employee)
        {
            if (ModelState.IsValid)
            {
                string employeeName = employee.Name;
                int employeeVal = employee.Value;
                string origEmpName = employee.prevName;
                int origEmpVal = employee.prevVal;
                switch (employeeName[0])
                {
                    case 'E':
                        employeeVal += 1;
                        break;
                    case 'G':
                        employeeVal += 10;
                        break;
                    default:
                        employeeVal += 100;
                        break;
                }
                var sql = "UPDATE Employees " +
                        "SET Name = @Name," +
                        "Value = @Value " +
                        "WHERE " +
                        "Name = @origName AND " +
                        "Value = @origVal";
                var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
                try
                {
                    using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                    {
                        connection.Open();

                        var queryCmd = new SqliteCommand(sql, connection);
                        queryCmd.Parameters.AddWithValue("@Name", employeeName);
                        queryCmd.Parameters.AddWithValue("@Value", employeeVal);
                        queryCmd.Parameters.AddWithValue("@origName", origEmpName);
                        queryCmd.Parameters.AddWithValue("@origVal", origEmpVal);
                        var rowInserted = queryCmd.ExecuteNonQuery();

                        Console.WriteLine($"The employee '{employeeName}' has been updated successfully.");

                    }
                }
                catch (SqliteException ex)
                {
                    return BadRequest(ex);
                }
            }
            return Ok("Employee " + employee.Name + "has been updated");
        }

        [HttpPost]
        [Route("/deleteEmployee")]
        public ActionResult deleteEmployee([FromBody] Employee employee) {
            if (ModelState.IsValid)
            {
                string employeeName = employee.Name;
                int employeeVal = employee.Value;
                var sql = "DELETE FROM employees " +
                    "WHERE " +
                    "Name = @Name AND " +
                    "Value = @Value";
                var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
                try
                {
                    using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                    {
                        connection.Open();

                        var queryCmd = new SqliteCommand(sql, connection);
                        queryCmd.Parameters.AddWithValue("@Name", employeeName);
                        queryCmd.Parameters.AddWithValue("@Value", employeeVal);
                        var rowDelete = queryCmd.ExecuteNonQuery();

                        Console.WriteLine($"The employee '{employeeName}' has been deleted successfully.");

                    }
                }
                catch (SqliteException ex)
                {
                    return BadRequest(ex);
                }
            }
            return Ok("Employee " + employee.Name + "has been deleted");
        }
    }
}
