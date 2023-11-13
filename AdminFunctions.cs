using BankNyBank.Data;
using BankNyBank.Models;
using BankNyBank.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank
{
    internal static class AdminFunctions
    {
        public static void AdminMenu()
        {
            using(BankContext context = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    List<User> users = DbHelper.GetAllUsers(context);
                    users.RemoveAt(0);

                    string pageHeader = "~~~~ Admin menu ~~~~";
                    string[] menuOptions =
                    {
                        "Show users",
                        "Create user",
                        "Remove user",
                        "Log out"
                    };

                    int command = MenuClass.DisplayAndGetMenuChoice(pageHeader, menuOptions);

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
            Console.Write("Enter user name: ");
            string userName = Console.ReadLine();

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
            Console.Clear();
            Console.WriteLine("Remove user menu");
            int i = 1;
            foreach (User user in users)
            {
                Console.WriteLine($"{i}. {user.Name}");
                i++;
            }

            Console.Write("Select user to remove: ");
            int removeIndex = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine($"Remove user \"{users[removeIndex].Name}\"?");
            Console.Write("Y/N: ");
            string yesNo = Console.ReadLine().ToLower();
            bool success = false;
            int countTries = 0;

            // Tries to remove user from database. If successful, success message is shown and user is removed.
            // Otherwise prints an error message, and clarifies that the user was not added.
            // The switch checks for the correct input 5 times and if the input is invalid, the admin is sent
            // back to the admin menu.
            while (!success)
            {
                switch (yesNo)
                {
                    case "y":
                        success = DbHelper.RemoveUser(context, users[removeIndex]);
                        break;
                    case "n":
                        Console.WriteLine("No user removed");
                        Console.WriteLine("Press enter to return to admin menu");
                        Console.ReadLine();
                        return;
                    default:
                        if (countTries > 4)
                        {
                            Console.WriteLine("Too many invalid inputs. Returning to menu...");
                            Thread.Sleep(2500);
                            return;
                        }
                        Console.WriteLine("Invalid input. Try again: ");
                        countTries++;
                        yesNo = Console.ReadLine();
                        break;
                }
            }

            if (success)
            {
                Console.WriteLine("Successfully removed user!");
            }
            else
            {
                Console.WriteLine("Failed to remove user");
            }
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
        }


    }
}
