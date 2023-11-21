using BankNyBank.Data;
using BankNyBank.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites
{
    internal class UserAuthentication
    {
        // Login processes will return either a user with access to accounts or an admin with acces to admin tasks
        public static User CheckLogIn(BankContext context)
        {
            while (true)
            {
                Console.Write("Enter user name: ");
                string userName = Console.ReadLine();

                User user = context.Users
                .Where(u => u.Name == userName)
                .SingleOrDefault();

                if (user != null)
                {

                    if (CheckPin(user))
                    {
                        return user;
                    }

                    if (Cooldown())
                    {
                        break;
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No such user in the vault. Try again");
                }
            }
            return null;
        }

        // Check to see if the user that put in the name knows the corresponding pin
        private static bool CheckPin(User user)
        {
            const int maxLoginAttempts = 3;

            Console.Clear();
            Console.WriteLine($"User '{user.Name}' found. Please enter the PIN.");

            for (int i = 1; i <= maxLoginAttempts; i++)
            {
                Console.Write("Enter PIN: ");
                string pin = Console.ReadLine();

                if (user.Pin == pin)
                {
                    return true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Invalid PIN. Try again. {maxLoginAttempts - i} left.");
                }
            }
            return false;
        }

        // Method to put a user in a timeout if they input the wrong pin three times
        private static bool Cooldown()
        {
            const int jailTime = 180; // 3 min

            Console.Clear();
            Console.WriteLine("You ran out of tries. Lockout timer initiated.");
            Console.CursorVisible = false;
            for (int cooldown = jailTime; cooldown >= 0; cooldown--)
            {
                Console.SetCursorPosition(0, 1);
                int minutes = cooldown / 60;
                int remainingcooldown = cooldown % 60;
                Console.WriteLine($"\nTry again in: {minutes:D2}m {remainingcooldown:D2}s");
                Thread.Sleep(1000); // Sleep for 1 second to refresh timer properly
            }
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Lockout is over. Press Enter to go back to the start screen:");
            Console.ReadLine();
            
            return true;
        }

        public static User FindUserByName(BankContext context, string userName)
        {
            return context.Users
                .Where(u => u.Name == userName)
                .SingleOrDefault();
        }
    }
}
