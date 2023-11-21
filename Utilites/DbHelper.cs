using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public static List<User> GetAllUsers(BankContext context)
        {
            List<User> users = context.Users.ToList();
            users.RemoveAt(0);
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

        public static void DisplayAccounts(BankContext context, User user)
        {
            var displayUserAccounts = context.Accounts
                    .Where(u => u.User.Id == user.Id)
                    .Select(a => new
                    {
                        a.Name,
                        a.AccountType,
                        Balance = $"{a.Balance:N2}",
                        a.Currency
                    })
                    .ToList();

            Console.Clear();

            if (displayUserAccounts.Count > 1) 
            {
                Console.WriteLine($"\n{user.Name}'s current accounts and balances:\n");
            }
            else if (displayUserAccounts.Count == 1)
            {
                Console.WriteLine($"\n{user.Name}'s current account and balance:\n");
            }
            else
            {
                Console.WriteLine($"\nNo accounts found for user {user.Name}.");
            }            

            foreach (var accountDetails in displayUserAccounts)
            {
                PrintOneAccount(accountDetails.Name, accountDetails.AccountType, accountDetails.Balance, accountDetails.Currency);
            }
            // Wait for user to press the ENTER key
            Console.WriteLine("\nPress ENTER to return to the main menu.");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Console.WriteLine("Returning to the main menu...");
            Thread.Sleep(1000);
            return;
        }

        // Save deposit
        public static bool DepositToAccount(BankContext context, Account account, double depositAmount)
        {
            // Try catch
            account.Balance += depositAmount;
            context.SaveChanges();
            return true;
        }

        // Save withdraw
        public static bool WithdrawFromAccount(BankContext context, Account account, double withdrawalAmount)
        {
            // Add try catch
            account.Balance -= withdrawalAmount;
            context.SaveChanges();
            return true;
        }

        public static void PrintOneAccount(string name, string accountType, string balance, string currency)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine($"Account: {name}");
            Console.WriteLine($"Account type: {accountType}");
            Console.WriteLine($"Balance: {balance} {currency}");
            Console.WriteLine("==============================================\n");
        }
    }
}
