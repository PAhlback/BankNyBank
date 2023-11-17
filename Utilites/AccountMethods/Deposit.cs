using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites.AccountMethods
{
    internal class Deposit : User
    {
        public static void DepositMenu(BankContext context, User user)
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
                int choice = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                if (choice <= displayUserAccounts.Count)
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
                        DbHelper.DepositToAccount(context, selectedAccount, depositAmount);
                        Console.WriteLine($"Deposit successful.");
                        Console.WriteLine($"New balance of account {selectedAccount.Name}: {selectedAccount.Balance:N2} {displayUserAccounts[choice - 1].Currency}");
                        Console.WriteLine("Press ENTER to return to menu");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid deposit amount.");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadKey();
                    }
                }
                else if (choice == displayUserAccounts.Count + 2)
                {
                    Console.WriteLine("Returning to the main menu...");
                    Thread.Sleep(1000);
                    return;
                }
            }
        }
    }
}
