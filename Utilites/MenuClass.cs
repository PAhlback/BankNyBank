using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites
{
    internal static class MenuClass
    {
        public static void MainMenu(BankContext context)
        {
            string pageHeader = $" ~~~~ Welcome to bankbank! ~~~~\n" +
                $"~~~~ Logging in or quitting ~~~~";
            string[] menuOptions =
            {
                "Login",
                "Quit"
            };

            while (true)
            {
                int choice = DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        // Check user login input method
                        User user = UserAuthentication.CheckLogIn(context);

                        DisplayMenu(context, user);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Closing down...");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        return;
                }
            }
        }

        static void UserMenu(BankContext context, User user)
        {
            string pageHeader = $"~~~~ Welcome, {user.Name} ~~~~";
            string[] menuOptions =
            {
                "View accounts and balance",
                "Transfer between accounts",
                "Withdraw",
                "Deposit",
                "Open new account",
                "Log out"
            };

            while (true)
            {
                int choice = DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                switch (choice)
                {
                    case 1:
                        DbHelper.DisplayAccounts(context, user);
                        break;
                    case 2:
                        // Transfer between accounts
                        Console.WriteLine("Not implemented");
                        break;
                    case 3:
                        // Withdraw from account
                        Console.WriteLine("Not implemented");
                        break;
                    case 4:
                        // Deposit to account
                        Console.WriteLine("Not implemented");
                        break;
                    case 5:
                        // Open new account
                        User.OpenNewAccount(context, user);
                        break;
                    case 6:
                        Console.WriteLine("Logging out...");
                        Thread.Sleep(1000);
                        return;
                }
                // Remove when all cases are implemented
                Console.ReadLine();

            }

        }

        static void DisplayMenu(BankContext context, User user)
        {
            if (user.Name == "admin")
            {
                AdminFunctions.AdminMenu();
            }
            else
            {
                UserMenu(context, user);
            }
        }



        public static int DisplayAndGetMenuChoice(string pageHeader, string[] menuoptions)
        {
            // Set the cursor visibility to false for a cleaner user interface.
            Console.CursorVisible = false;

            // Initialize the selected menu option index
            int menuHeight = menuoptions.Length;
            int y = 0;

            Console.Clear();
            Console.WriteLine(pageHeader);

            // Main loop for handling user input and menu navigation
            // Loop is versatile and can work with many different length menus
            while (true)
            {
                // Print the menu options with the arrow indicating the selected option.
                for (int i = 0; i < menuoptions.Length; i++)
                {
                    Console.SetCursorPosition(0, i + 2);
                    Console.Write(i == y ? "=>" : "  ");
                    Console.WriteLine(menuoptions[i]);
                }
                if (Console.KeyAvailable)
                {
                    // Takes input from user
                    var command = Console.ReadKey().Key;
                    if (command == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        return y + 1;
                    }
                    // Switch that incrementes or decrements the y axis of the arrow.
                    switch (command)
                    {
                        case ConsoleKey.UpArrow:
                            if (y > 0)
                            {
                                y--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (y < menuHeight - 1)
                            {
                                y++;
                            }
                            break;
                    }
                }
                // If no input is detected within 100 milliseconds, the loop starts over.
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
