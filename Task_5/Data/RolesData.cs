using System.Data.SqlClient;
using Utility;

namespace Data
{
    public  class RolesData
    {
        string connectionString = "Server=10.0.0.27;Database=SravanthiEmployeeDB;Integrated Security = True";

        public void AddRoleIntoDB(string? roleName, string? department, string? roleDescription, string? location)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Roles (RoleName, Department, RoleDescription, Location) VALUES (@RoleName, @Department, @RoleDescription, @Location)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@RoleDescription", roleDescription);
                    command.Parameters.AddWithValue("@Location", location);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DisplayRolesFromDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Roles";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (reader.HasRows)
                        {
                            int count = 1;
                            while (reader.Read())
                            {
                                Console.WriteLine($"{count}) Role Name: {reader.GetString(0)}, Department: {reader.GetString(1)}, Description: {reader.GetString(2)}, Location: {reader.GetString(3)}");
                                Console.WriteLine("====================================================================================================================================================");
                                count++;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(Prompts.NoRolesMessage);
                        }
                    }
                }
            }
        }
    }
}
