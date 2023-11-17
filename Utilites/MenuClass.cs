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

        public static void UserMenu(BankContext context, User user)
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
                        // Withdrawal
                        MenuClass.WithdrawMenu(context, user);
                        Console.WriteLine("Not implemented");
                        break;
                    case 4:
                        // Deposit
                        MenuClass.DepositMenu(context, user);
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
                    Console.Write(i == y ? "→ " : "  ");
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


        static void DepositMenu(BankContext context, User user)
        {
            string pageHeader = "Please choose account to deposit to:";

            // Get the user's accounts from the context
            var displayUserAccounts = context.Accounts
                    .Where(u => u.User.Id == user.Id)
                    .Select(a => new
                    {
                        a.Name,
                        Balance = $"{a.Balance:N2}",
                        a.Currency
                    })
                    .ToList();

            // Using an array to store menu options, including user's accounts and "Return to main menu"
            string[] menuOptions = new string[displayUserAccounts.Count + 2];

            // Getting menu options as accounts from the current user
            for (int i = 0; i < displayUserAccounts.Count; i++)
            {
                menuOptions[i] = $" {displayUserAccounts[i].Name}";
            }

            // Just to add a space between the listed accounts and "Return to main menu"
            menuOptions[displayUserAccounts.Count] = "";

            // "Return to main menu" option added at the end
            menuOptions[displayUserAccounts.Count + 1] = "Return to main menu";

            while (true)
            {
                int choice = DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                if (choice >= 1 && choice <= displayUserAccounts.Count)
                {
                    string selectedAccountName = displayUserAccounts[choice - 1].Name;
                    Console.WriteLine($"Selected account: {selectedAccountName}");

                    // Get the selected account from the context and display current balance
                    Account selectedAccount = context.Accounts.FirstOrDefault(a => a.User.Id == user.Id && a.Name == selectedAccountName);

                    Console.WriteLine($"Current balance: {selectedAccount.Balance:N2} {displayUserAccounts[choice - 1].Currency}");

                    Console.Write("Please enter deposit amount: ");
                    if (double.TryParse(Console.ReadLine(), out double depositAmount) && depositAmount > 0)
                    {
                        // Add funds to the chosen account
                        selectedAccount.Balance += depositAmount;
                        context.SaveChanges();
                        Console.WriteLine($"Deposit successful.");
                        Console.WriteLine($"New balance of account {selectedAccount.Name}: {selectedAccount.Balance:N2} {displayUserAccounts[choice - 1].Currency}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid deposit amount.");
                    }

                    Console.WriteLine("\nPress ENTER to return to the main menu.");
                    Console.ReadKey();
                    Console.WriteLine("Returning to main menu...");
                    Thread.Sleep(1000);
                    UserMenu(context, user);
                }
                else if (choice == displayUserAccounts.Count + 2)
                {
                    Console.WriteLine("Returning to the main menu...");
                    Thread.Sleep(1000);
                    UserMenu(context, user);
                }
            }
        }



        static void WithdrawMenu(BankContext context, User user)
        {
            string pageHeader = $"Please choose account to withdraw from:";

            // Get the user's accounts from the context
            var displayUserAccounts = context.Accounts
                    .Where(u => u.User.Id == user.Id)
                    .Select(a => new
                    {
                        a.Name,
                        Balance = $"{a.Balance:N2}",
                        a.Currency
                    })
                    .ToList();

            // Using an array to store menu options, including user's accounts and "Return to main menu"
            string[] menuOptions = new string[displayUserAccounts.Count + 2];

            // Getting menu options as accounts from current user
            for (int i = 0; i < displayUserAccounts.Count; i++)
            {
                menuOptions[i] = $" {displayUserAccounts[i].Name}";
            }

            // Just to add a space between the listed accounts and "Return to main menu"
            menuOptions[displayUserAccounts.Count] = "";

            // "Return to main menu" option added at the bottom of the options
            menuOptions[displayUserAccounts.Count + 1] = "Return to main menu";

            while (true)
            {
                // To track failed PIN attempts
                int failedAttempts = 0;

                int choice = DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                if (choice >= 1 && choice <= displayUserAccounts.Count)
                {
                    string selectedAccountName = displayUserAccounts[choice - 1].Name;
                    Console.WriteLine($"Selected account: {selectedAccountName}");

                    // Get the selected account from the context and display current balance
                    Account selectedAccount = context.Accounts.FirstOrDefault(a => a.User.Id == user.Id && a.Name == selectedAccountName);
                    while (failedAttempts < 3)
                    {
                        Console.Write("Please enter your PIN: ");
                        if (Console.ReadLine() == user.Pin)
                        {
                            Console.WriteLine($"PIN verified.");

                            Console.WriteLine($"Current balance: {selectedAccount.Balance:N2} {displayUserAccounts[choice - 1].Currency}");

                            Console.Write("Please enter withdrawal amount: ");
                            if (double.TryParse(Console.ReadLine(), out double withdrawalAmount) && withdrawalAmount < selectedAccount.Balance)
                            {
                                // Withdraw funds from the chosen account
                                selectedAccount.Balance -= withdrawalAmount;
                                context.SaveChanges();
                                Console.WriteLine($"Withdrawal successful.");
                                Console.WriteLine($"New balance of account {selectedAccount.Name}: {selectedAccount.Balance:N2} {displayUserAccounts[choice - 1].Currency}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a withdrawal amount within your balance.");
                            }
                            Console.WriteLine("\nPress ENTER to return to main menu.");
                            Console.ReadKey();
                            Console.WriteLine("Returning to the main menu...");
                            Thread.Sleep(1000);
                            UserMenu(context, user);
                        }
                        else
                        {
                            failedAttempts++;
                            Console.WriteLine($"Incorrect PIN. Attempts left: {3 - failedAttempts}");

                            if (failedAttempts == 3)
                            {
                                Console.WriteLine("Too many incorrect attempts. Logging out...");
                                Thread.Sleep(1000);
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Pin incorrect, try again.");
                            }
                        }
                    }
                }
                else if (choice == displayUserAccounts.Count + 2)
                {
                    Console.WriteLine("Returning to main menu...");
                    Thread.Sleep(1000);
                    UserMenu(context, user);
                }
            }
        }
    }
}

