using System.Data.SqlClient;
using Models;
using Utility;


namespace Data
{
    public class EmployeeData
    {
        string connectionString = "Server=10.0.0.27;Database=SravanthiEmployeeDB;Integrated Security=True";

        // Adding an employee into Database starts here
        public void AddEmployeeToDB(string? id, string? firstName, string? lastName, DateTime dateOfBirth, string? Email, string? Phone, DateTime joinDate, string? location, string? jobTitle, string? department, string? manager, string? project)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Employee (ID, FirstName, LastName, DateOfBirth, Email, Phone, JoinDate, Location, JobTitle, Department, Manager, Project) VALUES (@ID, @FirstName, @LastName, @DateOfBirth, @Email, @Phone, @JoinDate, @Location, @JobTitle, @Department, @Manager, @Project)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@JoinDate", joinDate);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@JobTitle", jobTitle);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Manager", string.IsNullOrEmpty(manager) ? DBNull.Value : (object)manager);
                    command.Parameters.AddWithValue("@Project", string.IsNullOrEmpty(project) ? DBNull.Value : (object)project);
                    command.ExecuteNonQuery();
                }
            }
        }
        // Adding an employee into Database ends here

        // Employee ID validation starts here
        public bool IdValidation(string query, string input)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@EmployeeId", input);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows || (input == "TZ0000"))
                    {
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }
                }
            }
        }
        // Employee ID validation ends here

        // Update employee data in Database starts here
        public int UpdateEmployeeDetails(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = @"UPDATE Employee 
                                   SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, 
                                       Email = @Email, Phone = @Phone, JoinDate = @JoinDate, Location = @Location, 
                                       JobTitle = @JobTitle, Department = @Department, Manager = @Manager, 
                                       Project = @Project 
                                   WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", employee.ID);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@JoinDate", employee.JoinDate);
                    command.Parameters.AddWithValue("@Location", employee.Location);
                    command.Parameters.AddWithValue("@JobTitle", employee.JobTitle);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@Manager", employee.Manager != null ? employee.Manager : DBNull.Value);
                    command.Parameters.AddWithValue("@Project", employee.Project != null ? employee.Project : DBNull.Value);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        // Update employee data in Database ends here


        // Delete an Employee in Database starts here
        public void DeleteEmployeeInDB(string query, string employeeIdToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeIdToDelete);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Prompts.EmployeeDeletedMessage);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Prompts.EmployeeIDErrorMessage);
                    }
                }
            }
        }
        // Delete an Employee in Database ends here

        // Data retreival from sql database starts here
        public List<Employee> GetEmployeesFromDB(string query, bool isIDRequired = true)
        {
            List<Employee> employees = new List<Employee>();
            string? employeeId = null;
            if (isIDRequired)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(Prompts.EmployeeIDInputMessage);
                employeeId = Console.ReadLine();
            }
            if (isIDRequired && !string.IsNullOrEmpty(employeeId))
            {
                query += $" WHERE ID = '{employeeId}'";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee
                                {
                                    //Instead of reader["ID"],reader["FirstName"],reader["LastName"] we can use index like reader[0],reader[1],reader[2]..,
                                    ID = reader["ID"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    JoinDate = Convert.ToDateTime(reader["JoinDate"]),
                                    Location = reader["Location"].ToString(),
                                    JobTitle = reader["JobTitle"].ToString(),
                                    Department = reader["Department"].ToString(),
                                    Manager = reader["Manager"].ToString(),
                                    Project = reader["Project"].ToString()
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            return employees;
        }
        // Data retreival from sql database ends here
    }
}
