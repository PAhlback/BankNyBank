using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites
{
    internal class UserAuthentication
    {
        public static User CheckLogIn(BankContext context)
        {
            const int maxLoginAttempts = 3;
            const int cooldownTimeInSeconds = 180; // 3 minutes

            while (true)
            {
                Console.Write("Enter user name: ");
                string userName = Console.ReadLine();

                // Find the user with the provided username
                User user = FindUserByName(context, userName);

                if (user != null)
                {

                    if (CheckPin(user, maxLoginAttempts))
                    {
                        // Correct PIN, return the user
                        return user;
                    }

                    Cooldown(cooldownTimeInSeconds);

                }
                else
                {
                    Console.WriteLine("No such user in the vault. Try again");
                }
            }
        }

        private static bool CheckPin(User user, int maxLoginAttempts)
        {
            // User with the provided username exists, check the PIN
            Console.WriteLine($"User '{user.Name}' found. Please enter the PIN.");

            for (int i = 1; i <= maxLoginAttempts; i++)
            {
                Console.Write("Enter PIN: ");
                string pin = Console.ReadLine();

                if (user.Pin == pin)
                {
                    // Correct PIN, return the true
                    return true;
                }
                else
                {
                    Console.WriteLine($"Invalid PIN. Try again. {i} of 3 attemps left.");
                }
            }

            return false;
        }

        private static void Cooldown(int cooldownTimeInSeconds)
        {
            for (int seconds = cooldownTimeInSeconds; seconds >= 0; seconds--)
            {
                Console.Clear();
                int minutes = seconds / 60;
                int remainingSeconds = seconds % 60;
                Console.WriteLine($"Cooldown: {minutes:D2}m {remainingSeconds:D2}s");
                Thread.Sleep(1000); // Sleep for 1 second
            }
            Console.WriteLine("Your timeout is over. Press Enter to go back to the login screen:");
            Console.ReadLine();
        }

        public static User FindUserByName(BankContext context, string userName)
        {
            // Find the user with the provided username
            return context.Users
                .Where(u => u.Name == userName)
                .SingleOrDefault();
        }
    }
}
