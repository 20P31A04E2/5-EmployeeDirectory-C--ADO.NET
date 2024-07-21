using Utility;
using Data;

namespace Services
{
    public class RolesManagementMenu
    {
        EmployeeManagementMenu AddRoles = new EmployeeManagementMenu();
        RolesData roleData = new RolesData();

        //Role Management starts here
        public void RoleManagement()
        {
            bool isRoleMenu = true;
            while (isRoleMenu)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Prompts.RoleMenu);
                Console.Write(Prompts.SelectedOption);
                int roleMenuChoice;
                if (!int.TryParse(Console.ReadLine(), out roleMenuChoice) || roleMenuChoice < 1 || roleMenuChoice > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Prompts.InvalidMessage);
                    continue;
                }
                switch ((RoleManagementMenu)roleMenuChoice)
                {
                    case RoleManagementMenu.AddRole:
                        AddRole();
                        break;
                    case RoleManagementMenu.DisplayAll:
                        DisplayAll();
                        break;
                    case RoleManagementMenu.IsRoleMenu:
                        isRoleMenu = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Prompts.InvalidMessage);
                        break;
                }
            }
        }
        //Role Management ends here

        //Add role starts here
        private void AddRole()
        {
            Console.WriteLine("\nEnter the role details to add: ");
            string? roleName = AddRoles.InputValidation("string", "Enter Role Name: ");
            string? department = AddRoles.InputValidation("string", "Enter Department Name: ");
            string? roleDescription = AddRoles.InputValidation("stringornull", "Enter Role Description: ");
            string? location = AddRoles.InputValidation("string", "Enter Location: ");

            roleData.AddRoleIntoDB(roleName, department, roleDescription, location);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Prompts.RoleAddedMessage);
        }
        //Add role ends here


        //Display all start here
        private void DisplayAll()
        {
            roleData.DisplayRolesFromDB();
        }
        //Display all ends here
    }
}