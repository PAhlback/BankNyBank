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
            //UserMenu(context);
        }

        static void UserMenu(BankContext context, User user)
        {

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
    }
}
