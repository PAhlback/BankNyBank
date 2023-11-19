﻿using System;
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
                        Balance = $"{a.Balance:N2}",
                        a.Currency
                    })
                    .ToList();

            Console.Clear();
            Console.WriteLine("\nYour current accounts and balance:\n");

            foreach (var accountDetails in displayUserAccounts)
            {
                PrintOneAccount(accountDetails.Name, accountDetails.Balance, accountDetails.Currency);
            }
            Console.WriteLine("Press ENTER to return to main menu...");
            Console.ReadLine();
        }

        // GetAccount

        // Save deposit/alter balance
        public static bool DepositToAccount(BankContext context, Account account, double depositAmount)
        {
            // Try catch
            account.Balance += depositAmount;
            context.SaveChanges();
            return true;
        }

        // Save withdraw/alter balance
        public static bool WithdrawFromAccount(BankContext context, Account account, double withdrawalAmount)
        {
            // Add try catch
            account.Balance -= withdrawalAmount;
            context.SaveChanges();
            return true;
        }

        public static void PrintOneAccount(string name, string balance, string currency)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine($"Account: {name}");
            Console.WriteLine($"Balance: {balance} {currency}");
            Console.WriteLine("==============================================\n");
        }
    }
}
