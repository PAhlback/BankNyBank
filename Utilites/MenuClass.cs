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
            // Add while loop
            Console.WriteLine("Welcome to bankbank!");
            Console.WriteLine("Please log in");

            

            // Check user log in input method

            
            // Code here for user login ******
            UserMenu(context);
            
        }

        static void UserMenu(BankContext context)
        {

        }

        static User CheckLogIn(BankContext context)
        {
            while (true)
            {
                Console.Write("Enter user name: ");
                string userName = Console.ReadLine();

                Console.Write("Enter pin: ");
                string pin = Console.ReadLine();

                User user = context.Users
                    .Where(u => u.Name == userName)
                    .SingleOrDefault();

                if(user == null)
                {

                }

                Console.WriteLine("No such user in the vault. Try again");

            }
            
        } 
    }
}
