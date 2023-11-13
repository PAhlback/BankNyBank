using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BankNyBank.Data;
using BankNyBank.Models;

namespace BankNyBank.Utilites
{
    internal static class DbHelper
    {


        public static void ReturnToMainMenuMessage()
        {
            Console.WriteLine("Press ENTER to return to the main menu...");
        }



        public static List<User> GetAllUsers(BankContext context)
        {
            List<User> users = context.Users.ToList();
            return users;
        }


        public static bool AddUser(BankContext context, User user)
        {
            context.Users.Add(user);
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding user: {e}");
                return false;
            }
            return true;
        }


        public static bool RemoveUser(BankContext context, User user)
        {
            context.Users.Remove(user);
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing user: {e}");
                return false;
            }
            return true;
        }


        public static void AddAccount(BankContext context, User user)
        {
            string accountName;

            /* Enforce the user to choose a name for the account
             * Prompt the user until valid input is provided */
            do
            {
                Console.WriteLine("Enter a name for the account (minimum 3 characters):");
                accountName = Console.ReadLine();

                // Check if the entered name is valid
                if (string.IsNullOrWhiteSpace(accountName) || accountName.Length < 3)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    // Check if the desired account name already exists in the database
                    if (context.Accounts.Any(a => a.Name == accountName))
                    {
                        Console.Clear();
                        Console.WriteLine("Account name already exists. Please choose a different name.");
                        accountName = null; // Set to null to repeat the loop
                    }
                }
            } while (string.IsNullOrWhiteSpace(accountName) || accountName.Length < 3);

            /* Enforce the user to choose a currency for the account
             * Prompts the user until valid input is provided */
            string currency;
            do
            {
                Console.WriteLine("Choose a currency for the account (SEK, EUR, USD):");
                currency = Console.ReadLine().ToUpper();

                if (currency != "SEK" && currency != "EUR" && currency != "USD")
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input.");
                }
            } while (currency != "SEK" && currency != "EUR" && currency != "USD");

            Account userAccount = new Account
            {
                UserId = user.Id,
                Name = accountName,
                Balance = 0,
                Currency = currency
            };

            context.Accounts.Add(userAccount);
            context.SaveChanges();

            Console.WriteLine($"Successfully added account: {accountName}\n\n");
            ReturnToMainMenuMessage();
        }



        public static void DisplayAccounts(BankContext context)
        {
            var displayUserAccounts = context.Accounts
                    .Select(a => new
                    {
                        a.Name,
                        Balance = $"{a.Balance:N0}",
                        a.Currency
                    })
                    .ToList();

            Console.Clear();
            Console.WriteLine("\nYour current accounts and balance:\n");

            foreach (var accountDetails in displayUserAccounts)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine($"Account: {accountDetails.Name}");
                Console.WriteLine($"Balance: {accountDetails.Balance} {accountDetails.Currency}");
                Console.WriteLine("==============================================\n");
            }
            ReturnToMainMenuMessage();
        }
    }
}


