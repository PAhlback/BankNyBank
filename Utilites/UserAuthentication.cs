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

        // Replace user PIN input with '*'
        public static string HidePin()
        {
            StringBuilder pin = new StringBuilder();

            while (true)
            {
                // Use 'true' to hide which key is being pressed
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Check if user press Backspace or Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pin.Append(key.KeyChar);
                    Console.Write('*');
                }
                else if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
                {
                    // Delete the last user input
                    pin.Length -= 1;

                    /* Visual representation of the deletion:
                       Move cursor back one position, 
                       overwrite '*' with a space character,
                       move the cursor back one position.*/
                    Console.Write("\b \b");
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            return pin.ToString();
        }

        // Method to put a user in a timeout if they input the wrong pin three times
        private static void Cooldown()
        {
            const int jailTime = 180; // 3 min

            Console.Clear();
            Console.WriteLine("You ran out of tries. Lockout timer initiated.");
            for (int cooldown = jailTime; cooldown >= 0; cooldown--)
            {
                Console.SetCursorPosition(0, 1);
                int minutes = cooldown / 60;
                int remainingcooldown = cooldown % 60;
                Console.WriteLine($"\nTry again in: {minutes:D2}m {remainingcooldown:D2}s");
                Thread.Sleep(1000); // Sleep for 1 second to refresh timer properly
            }
            Console.WriteLine("Lockout is over. Press Enter to go back to the start screen:");
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
