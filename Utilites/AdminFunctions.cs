using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites
{
    internal static class AdminFunctions
    {
        public static void AdminMenu()
        {
            using (BankContext context = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    List<User> users = DbHelper.GetAllUsers(context);

                    string pageHeader = "~~~~ Admin menu ~~~~";
                    string[] menuOptions =
                    {
                        "Show users",
                        "Create user",
                        "Remove user",
                        "Log out"
                    };

                    int command = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);

                    switch (command)
                    {
                        case 1:
                            DisplayUsers(context, users);
                            break;
                        case 2:
                            CreateUser(context);
                            break;
                        case 3:
                            RemoveUser(context, users);
                            break;
                        case 4:
                            Console.WriteLine("\nFarewell!");
                            Thread.Sleep(1000);
                            return;
                        default:
                            Console.WriteLine($"Unknown command: {command}");
                            break;
                    }
                }
            }
        }

        private static void DisplayUsers(BankContext context, List<User> users)
        {
            Console.WriteLine("Current users in system: ");
            foreach (User user in users)
            {
                Console.WriteLine($"{user.Name}");
            }
            Console.WriteLine($"\nTotal number of users = {users.Count()}");
            Console.WriteLine("Press enter to return to admin menu");
            Console.ReadLine();
        }

        private static void CreateUser(BankContext context)
        {
            Console.Clear();

            Console.WriteLine("Create user");
            Console.WriteLine("Enter to return to admin menu");
            Console.WriteLine("Username must be longer than 3 characters");
            Console.Write("\nEnter username: ");
            string userName = Console.ReadLine();

            if (userName == "")
            {
                Console.WriteLine("Returning to admin menu");
                Thread.Sleep(630);
                return;
            }

            while (context.Users.Any(u => u.Name == userName) || userName.Length < 3)
            {
                Console.WriteLine("Username already exists, or is shorter than 3 characters");
                Console.Write("Enter user name: ");
                userName = Console.ReadLine();
            }

            Random random = new Random();
            string pin = random.Next(1000, 10000).ToString();

            User newUser = new User()
            {
                Name = userName,
                Pin = pin
            };
            bool success = DbHelper.AddUser(context, newUser);
            if (success)
            {
                Console.WriteLine($"Created user {userName} with pin {pin}");
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {userName}");
            }
            Console.WriteLine("Press enter to return to admin menu");
            Console.ReadLine();
        }

        private static void RemoveUser(BankContext context, List<User> users)
        {
            string pageHeader = "~~~~ Select user to remove ~~~~";
            string[] menuOptions = new string[users.Count + 2];
            menuOptions[users.Count] = "";
            menuOptions[users.Count + 1] = "Return to main menu";

            int i = 0;
            foreach (User user in users)
            {
                menuOptions[i] = user.Name;
                i++;
            }

            int removeIndex = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions) - 1;

            pageHeader = $"Remove user {users[removeIndex].Name}?";
            string[] menuOptions2 = { "Yes", "No" };

            int validateRemove = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions2);

            bool success = false;

            /* Tries to remove user from database. If successful, success message is shown and user is removed.
               Otherwise prints an error message, and clarifies that the user was not added.
               The switch checks for the correct input 5 times and if the input is invalid, the admin is sent
               back to the admin menu.*/
            while (!success)
            {
                if (validateRemove == 1 && removeIndex <= users.Count)
                {
                    success = DbHelper.RemoveUser(context, users[removeIndex]);
                    if (success)
                    {
                        Console.WriteLine("Successfully removed user!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Failed to remove user");
                    }
                }
                else if (validateRemove == 2)
                {
                    return;
                }
            }
            Console.WriteLine("Press enter to return to menu");
            Console.ReadKey();
            return;
        }


    }
}
