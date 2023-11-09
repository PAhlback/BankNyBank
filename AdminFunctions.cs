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
        public static void DoAdminTasks()
        {
            using(BankContext context = new BankContext())
            {
                while (true)
                {
                    Console.Clear();
                    List<User> users = DbHelper.GetAllUsers(context);

                    Console.WriteLine("Current users in system: ");
                    foreach (User user in users)
                    {
                        Console.WriteLine($"{user.Name}");
                    }
                    Console.WriteLine($"Total number of users = {users.Count()}");
                    Console.WriteLine("c to create new user");
                    Console.WriteLine("r to remove user");
                    Console.WriteLine("x to exit");
                    Console.Write("Enter command: ");
                    string command = Console.ReadLine().ToLower();

                    switch(command)
                    {
                        case "c":
                            CreateUser(context);
                            break;
                        case "r":
                            RemoveUser(context, users);
                            break;
                        case "x":
                            Console.WriteLine();
                            Console.WriteLine("Good bye!");
                            Thread.Sleep(1000);
                            return;
                        default:
                            Console.WriteLine($"Unknown command: {command}");
                            break;
                    }
                }
            }
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
