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


