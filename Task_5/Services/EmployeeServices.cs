using Models;
using System.Globalization;
using System.Text.RegularExpressions;
using Utility;
using Data;

namespace Services
{

    public class EmployeeManagementMenu
    {
        EmployeeData empData = new EmployeeData();

        // Employee Management starts here
        public void EmployeeManagement()
        {
            bool isEmployeeManagement = true;
            while (isEmployeeManagement)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Prompts.EmployeeMenu);
                Console.Write(Prompts.SelectedOption);
                int employeeMenuChoice;
                if (!int.TryParse(Console.ReadLine(), out employeeMenuChoice) || employeeMenuChoice < 1 || employeeMenuChoice > 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Prompts.InvalidMessage);
                    continue;
                }
                switch ((EmployeeMenu)employeeMenuChoice)
                {
                    case EmployeeMenu.AddEmployee:
                        AddEmployee();
                        break;
                    case EmployeeMenu.ViewAllEmployees:
                        ViewAllEmployees();
                        break;
                    case EmployeeMenu.ViewAnEmployee:
                        ViewAnEmployee();
                        break;
                    case EmployeeMenu.EditEmployee:
                        EditEmployee();
                        break;
                    case EmployeeMenu.DeleteEmployee:
                        DeleteEmployee();
                        break;
                    case EmployeeMenu.IsEmployeeManagement:
                        isEmployeeManagement = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Prompts.InvalidMessage);
                        break;
                }
            }
        }
        // Employmee Management ends here

        // Adding an Employee
        private void AddEmployee()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nEnter employee details:");
            string? id = InputValidation("id", "Enter ID of the Employee: ");
            string? firstName = InputValidation("string", "Enter First Name of the Employee: ");
            string? lastName = InputValidation("string", "Enter Last Name of the Employee: ");
            DateTime dateOfBirth = DateValidation("Enter Date of Birth (DD-MM-YYYY) of the Employee: ");
            string? Email = InputValidation("email", "Enter Email of the Employee: ");
            string? Phone = InputValidation("phone", "Enter Phone number of the Employee: ");
            DateTime joinDate = DateValidation("Enter Joining Date (DD-MM-YYYY) of the Employee: ", false);
            string? location = InputValidation("string", "Enter Location of the Employee: ");
            string? jobTitle = InputValidation("string", "Enter Job Title of the Employee: ");
            string? department = InputValidation("string", "Enter Department of the Employee: ");
            string? manager = InputValidation("stringornull", "Enter Manager name for the Employee: ");
            string? project = InputValidation("stringornull", "Enter Project Name for the Employee: ");

            empData.AddEmployeeToDB(id, firstName, lastName, dateOfBirth, Email, Phone, joinDate, location, jobTitle, department, manager, project);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Prompts.EmployeeAddedMessage);
        }
        // Adding employee ends here

        // Input validations starts here
        public string? InputValidation(string inputField, string prompt)
        {
            string? pattern;
            string? input = null;

            switch (inputField)
            {
                case "id":
                    pattern = @"^TZ\d{4}$";
                    input = IsValid(inputField, prompt, pattern);
                    break;
                case "string":
                    pattern = @"^[A-Za-z\s]+$";
                    input = IsValid(inputField, prompt, pattern);
                    break;
                case "email":
                    pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                    input = IsValid(inputField, prompt, pattern);
                    break;
                case "phone":
                    pattern = @"^\d{10}$";
                    input = IsValid(inputField, prompt, pattern);
                    break;
                case "stringornull":
                    pattern = @"^[A-Za-z\s.]+$";
                    input = IsValid(inputField, prompt, pattern, true);
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
            return input;
        }

        private string? IsValid(string inputField, string prompt, string pattern, bool skipValidations = false)
        {
            string? input = null;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(prompt);
                input = Console.ReadLine();
                string query = "SELECT ID FROM Employee WHERE ID = @EmployeeId";
                if (inputField == "id" && !string.IsNullOrEmpty(input) && Regex.IsMatch(input, pattern))
                {
                    if(empData.IdValidation(query, input))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a unique ID.");
                    }
                    else
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, pattern))
                {
                    break;
                }
                else if (skipValidations && string.IsNullOrEmpty(input))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format(Prompts.ValidationErrorMessage, inputField));
                }
            }
            return input;
        }


        private DateTime DateValidation(string prompt, bool allowNullInput = true)
        {
            DateTime date;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (allowNullInput)
                    {
                        return new DateTime(1800, 01, 01);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Prompts.DateErrorMessage);
                        continue;
                    }
                }
                else if (DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Prompts.DateErrorMessage);
                }
            }
        }
        // Input validations ends here
        // Add employees ends here

        // View all employees starts here
        private void ViewAllEmployees()
        {
            Console.ForegroundColor = ConsoleColor.White;
            string query = "SELECT ID, FirstName, LastName, DateOfBirth, Email, Phone, JoinDate, Location, JobTitle, Department, Manager, Project FROM Employee";
            List<Employee> employees = empData.GetEmployeesFromDB(query, false);
            if (employees.Any())
            {
                Console.WriteLine();
                int count = 1;
                foreach (var employee in employees)
                {
                    string name = employee.FirstName + " " + employee.LastName;
                    Console.WriteLine($"{count})ID: {employee.ID}, Name: {name}, JobTitle: {employee.JobTitle}, Department: {employee.Department}, DateOfBirth: {employee.DateOfBirth.ToString("dd-MM-yyyy")}, Email: {employee.Email}, Location: {employee.Location}, Joining Date: {employee.JoinDate.ToString("dd-MM-yyyy")}, Manager Name: {employee.Manager}, Project Name: {employee.Project}");
                    Console.WriteLine("====================================================================================================================================================");
                    count++;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Prompts.NoEmployeesMessage);
            }
        }
        // View all employees ends here

        // Search for an employee starts here
        private void ViewAnEmployee()
        {
            string query = "SELECT ID, FirstName, LastName, DateOfBirth, Email, Phone, JoinDate, Location, JobTitle, Department, Manager, Project FROM Employee";
            List<Employee> employees = empData.GetEmployeesFromDB(query);
            Employee? employee = employees.SingleOrDefault();
            if (employee != null)
            {
                Console.WriteLine();
                string name = employee.FirstName + " " + employee.LastName;
                Console.WriteLine($"ID: {employee.ID}, Name: {name}, JobTitle: {employee.JobTitle}, Department: {employee.Department}, DateOfBirth: {employee.DateOfBirth.ToString("dd-MM-yyyy")}, Email: {employee.Email}, Location: {employee.Location}, Joining Date: {employee.JoinDate.ToString("dd-MM-yyyy")}, Manager Name: {employee.Manager}, Project Name: {employee.Project}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Prompts.EmployeeIDErrorMessage);
            }
        }
        // Search for an employee ends here

        // Edit employee details starts here
        private void EditEmployee()
        {
            string query = "SELECT ID, FirstName, LastName, DateOfBirth, Email, Phone, JoinDate, Location, JobTitle, Department, Manager, Project FROM Employee";
            List<Employee> employees = empData.GetEmployeesFromDB(query);
            Employee? employee = employees.SingleOrDefault();

            if (employee != null)
            {
                string name = employee.FirstName + " " + employee.LastName;
                Console.WriteLine($"ID: {employee.ID}, Name: {name}, DateOfBirth: {employee.DateOfBirth.ToString("dd-MM-yyyy")}, Email: {employee.Email}, Phone: {employee.Phone}, JoinDate: {employee.JoinDate.ToString("dd-MM-yyyy")}, Location: {employee.Location}, JobTitle: {employee.JobTitle}, Department: {employee.Department}, Manager name: {employee.Manager}, Project name: {employee.Project}");

                bool continueEditing = true;
                while (continueEditing)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Prompts.EditMenu);
                    Console.Write(Prompts.SelectedOption);
                    int editChoice;
                    if (!int.TryParse(Console.ReadLine(), out editChoice) || editChoice < 1 || editChoice > 13)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Prompts.InvalidMessage);
                        continue;
                    }

                    switch ((EditEmployeeMenu)editChoice)
                    {
                        case EditEmployeeMenu.FirstName:
                            employee.FirstName = InputValidation("string", "Enter First Name of the Employee: ");
                            break;
                        case EditEmployeeMenu.LastName:
                            employee.LastName = InputValidation("string", "Enter Last Name of the Employee: ");
                            break;
                        case EditEmployeeMenu.DateOfBirth:
                            employee.DateOfBirth = DateValidation("Enter Date of Birth (DD-MM-YYYY) of the Employee: ");
                            break;
                        case EditEmployeeMenu.Email:
                            employee.Email = InputValidation("email", "Enter Email of the Employee: ");
                            break;
                        case EditEmployeeMenu.Phone:
                            employee.Phone = InputValidation("phone", "Enter Phone number of the Employee: ");
                            break;
                        case EditEmployeeMenu.JoinDate:
                            employee.JoinDate = DateValidation("Enter Joining Date (DD-MM-YYYY) of the Employee: ", false);
                            break;
                        case EditEmployeeMenu.Location:
                            employee.Location = InputValidation("string", "Enter Location of the Employee: ");
                            break;
                        case EditEmployeeMenu.JobTitle:
                            employee.JobTitle = InputValidation("string", "Enter Job Title of the Employee: ");
                            break;
                        case EditEmployeeMenu.Department:
                            employee.Department = InputValidation("string", "Enter Department of the Employee: ");
                            break;
                        case EditEmployeeMenu.Manager:
                            employee.Manager = InputValidation("stringornull", "Enter Manager name for the Employee: ");
                            break;
                        case EditEmployeeMenu.Project:
                            employee.Project = InputValidation("stringornull", "Enter Project Name for the Employee: ");
                            break;
                        case EditEmployeeMenu.ContinueEditing:
                            continueEditing = false;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(Prompts.InvalidMessage);
                            break;
                    }
                }
                int rowsAffected = empData.UpdateEmployeeDetails(employee);
                if (rowsAffected > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Employee details updated successfully.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No employee details were updated. Please check...");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Prompts.EmployeeIDErrorMessage);
            }
        }

        // Edit employee details ends here

        // Delete an Employee starts here
        private void DeleteEmployee()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Prompts.EmployeeIDInputMessage);
            string? employeeIdToDelete = Console.ReadLine();
            string query = "DELETE FROM Employee WHERE ID = @EmployeeId";
            empData.DeleteEmployeeInDB(query, employeeIdToDelete);
        }
        // Delete an employee ends here
    }
}