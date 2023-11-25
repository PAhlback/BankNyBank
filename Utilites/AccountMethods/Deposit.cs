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
            string pageHeader = "Please select account for the deposit:";

            // Get the user's accounts from the context and save them to a list.    
            var displayUserAccounts = context.Accounts
                    .Where(u => u.User.Id == user.Id)
                    .Select(a => new
                    {
                        a.Name,
                        Balance = $"{a.Balance:N2}",
                        a.Currency
                    })
                    .ToList();

            /* Use an array equal to number of user accounts +2 to store menu options.
               Add extra length to fit spacing and "Return to main menu" choice at the end. */
            string[] menuOptions = new string[displayUserAccounts.Count + 2];

            // Get menu options as accounts from the previous array. [i] equals the index of the array.
            for (int i = 0; i < displayUserAccounts.Count; i++)
            {
                menuOptions[i] = $" {displayUserAccounts[i].Name}";
            }

            /* Add an empty string at the index equal to the count of displayUserAccounts.
               Since arrays begin at 0 (zero-indexed) the empty string will always be assigned after the last account.

               Example with three accounts: Index will be equal to 3.
               [ " Account1", " Account2", " Account3", "", "" ]
            
               Space between the listed accounts and "Return to main menu"
               make it easier to distinguish between accounts and user option */
            menuOptions[displayUserAccounts.Count] = "";

            /* "Return to main menu" option added at the end of the menu options.
                Same logic as in the previous example, except we add 1 to place it at the very end. */
            menuOptions[displayUserAccounts.Count + 1] = "Return to main menu";

            while (true)
            {
                // Call DisplayAndGetMenuChoice to print the menu and save the user choice as an int.
                int choice = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                // Use the value of variable 'choice' to be able to determine which account the user will chose.
                if (choice <= displayUserAccounts.Count)
                {
                    /* Determine which account the user chose in the menu and prints it to the console as a confirmation.
                       [choice - 1] is used since lists are zero-indexed and the user choice is determined from the menu choices that start from 1 */
                    string selectedAccountName = displayUserAccounts[choice - 1].Name;
                    Console.WriteLine($"\nSelected account: {selectedAccountName}");
                    int characterCount = (selectedAccountName.Length + 18);
                    for (int j = 0; j < characterCount; j++)
                    {
                        Console.Write("=");
                    }

                    /* Get the selected account from the context and display current balance.
                       Since account names must be unique at creation, we know that we will retrieve the correct one */
                    Account selectedAccount = context.Accounts.FirstOrDefault(a => a.User.Id == user.Id && a.Name == selectedAccountName);
                    Console.WriteLine($"\n\nCurrent balance: {selectedAccount.Balance:N2} {selectedAccount.Currency}");

                    // Add validation and re-prompts user if input is invalid.
                    bool validInput = false;
                    while (!validInput)
                    {
                        Console.Write("\nPlease enter deposit amount: ");
                        // Check if the input from the user is correct and parsing it to a double.
                        if (double.TryParse(Console.ReadLine(), out double depositAmount) && depositAmount > 0)
                        {
                            // Add funds to the chosen account and confirm the deposit to the user.
                            DbHelper.DepositToAccount(context, selectedAccount, depositAmount);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Deposit successful.");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\nNew balance of account {selectedAccount.Name}: {selectedAccount.Balance:N2} {selectedAccount.Currency}");
                            validInput = true;
                            Console.WriteLine("\nPress ENTER to return to the main menu.");
                            // Wait for user to press the ENTER key
                            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                            Console.WriteLine("Returning to the main menu...");
                            Thread.Sleep(1000);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid deposit amount.");
                        }
                    }
                }
                else if (choice == displayUserAccounts.Count + 2)
                {
                    Console.WriteLine("Returning to the main menu...");
                    // Add a slight delay so the user has time to read to message
                    Thread.Sleep(1000);
                    return;
                }
            }
        }
    }
}

