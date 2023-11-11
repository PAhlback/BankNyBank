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
                // This prints the welcome message a second time. Keep this one or keep the one in MainMenu?
                //Console.WriteLine("Welcome to bankbank!");
                //Console.WriteLine("Please log in");
                Console.Write("Enter user name: ");
                string userName = Console.ReadLine();

                User user = FindUserByName(context, userName);

                if (user != null)
                {

                    if (CheckPin(user))
                    {
                        return user;
                    }

                    Cooldown();

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No such user in the vault. Try again");
                }
            }
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
                    Console.WriteLine($"Invalid PIN. Try again. {maxLoginAttempts - i} left.");
                }
            }

            return false;
        }

        // Method to put a user in a timeout if they input the wrong pin three times
        private static void Cooldown()
        {
            const int cooldownTimeInSeconds = 180;

            Console.Clear();
            Console.WriteLine("You ran out of tries. Jail time till timmer reaches zero");
            for (int seconds = cooldownTimeInSeconds; seconds >= 0; seconds--)
            {
                Console.SetCursorPosition(0, 1);
                int minutes = seconds / 60;
                int remainingSeconds = seconds % 60;
                Console.WriteLine($"Cooldown: {minutes:D2}m {remainingSeconds:D2}s");
                Thread.Sleep(1000); // Sleep for 1 second
            }
            Console.WriteLine("Your jail time is over. Press Enter to go back to the start screen:");
            Console.ReadLine();
            Console.Clear();
        }


        public static User FindUserByName(BankContext context, string userName)
        {
            return context.Users
                .Where(u => u.Name == userName)
                .SingleOrDefault();
        }
    }
}
