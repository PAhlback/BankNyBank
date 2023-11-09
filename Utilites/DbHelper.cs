using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void DisplayAccounts(BankContext context)
        {
            {
                var displayUserAccounts = context.Accounts
                    .Select(a => new
                    {
                        a.Name,
                        a.Balance
                    })
                    .ToList();

                Console.Clear();

                foreach (var accountDetails in displayUserAccounts)
                {
                    Console.WriteLine();
                    Console.WriteLine("Your current accounts and balance:");
                    Console.WriteLine();
                    Console.WriteLine("==============================================");
                    Console.WriteLine($"Account name: {accountDetails.Name}\t Balance: {accountDetails.Balance}");
                    Console.WriteLine("==============================================");
                    Console.WriteLine();
                }
            }
        }
    }
}
