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
            // Run this on your first run
            //User admin = new User()
            //{
            //    Name = "admin",
            //    Pin = "1234"
            //};
            //context.Users.Add(admin);
            //context.SaveChanges();
            //Console.ReadLine();

            // Add while loop
            Console.WriteLine("Welcome to bankbank!");
            Console.WriteLine("Please log in");

            // Check user log in input method
            User user = UserAuthentication.CheckLogIn(context);

            // Code here for user login ******
            DisplayMenu(context, user);
        }

        static void UserMenu(BankContext context, User user)
        {
            string pageHeader = $"~~~~ Welcome, {user.Name} ~~~~" ;
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

                switch (choice)
                {
                    case 1:
                        DbHelper.DisplayAccounts(context);
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
                        Console.WriteLine("Not implemented");
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

        static void AddAccountToUser(BankContext context, User user)
        {

            // Add an account for the user
            Account userAccount = new Account { UserId = user.Id, Name = "Default", Balance = 0.0 };
            context.Accounts.Add(userAccount);
            context.SaveChanges();
        }

        // Method that allows for selecting in the menu using arrow keys. Can (probably) not be broken
        // due to invalid input. Scales with amount of menu options. 
        public static int DisplayAndGetMenuChoice(string pageHeader, string[] menuoptions)
        {
            // Menu choice index starts at 1 (corresponding row, including page header).
            int menuHeight = menuoptions.Length;
            int y = 1;

            Console.SetCursorPosition(0, 0);

            // Method that clears the previous menu, then prints the page header and sets the arrow at the
            // first position.
            WriteMethod(pageHeader, y);

            // This while loop takes the user input and, if valid, moves the arrow in the options menu.
            // It prints the menu options, then takes the input. Input can only be arrow up and down, or enter.
            // If the input is enter the method returns that choice (index starts at 1). If it's up or down, it 
            // moves the arrow. The arrow can not go outside the span for the options. It can only move between row 1
            // and the last row with an option (so if there are 4 options, the arrow can move between row 1-4).
            while (true)
            {
                // Prints the menu options
                for (int i = 0; i < menuoptions.Length; i++)
                {
                    Console.SetCursorPosition(3, i + 1);
                    Console.WriteLine(menuoptions[i]);
                }
                if (Console.KeyAvailable)
                {
                    // Takes input from user
                    var command = Console.ReadKey().Key;
                    if (command == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        return y;
                    }
                    // Switch that incrementes or decrements the y axis of the arrow.
                    switch (command)
                    {
                        case ConsoleKey.UpArrow:
                            if (y > 1)
                            {
                                y--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (y < menuHeight)
                            {
                                y++;
                            }
                            break;
                    }
                    WriteMethod(pageHeader, y);
                }
                // If no input is detected within 100 milliseconds, the loop starts over.
                else
                {
                    Thread.Sleep(100);
                }
            }

            // Method that clears the menu and prints a new menu with the arrow at the new place.
            static void WriteMethod(string pageHeader, int y = 1)
            {
                Console.Clear();
                Console.WriteLine(pageHeader);
                Console.SetCursorPosition(0, y);
                Console.Write("=>");
            }
        }
    }
}
