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
                Console.Write("Username: ");
                string userName = Console.ReadLine();

                User user = FindUserByName(context, userName);

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
                    Console.WriteLine("No such user in the vault. Try again");
                }
            }
            return null;
        }

        // Check to see if the user that put in the name knows the corresponding pin
        private static bool CheckPin(User user)
        {
            const int maxLoginAttempts = 3;

            Console.WriteLine($"\nUser '{user.Name}' found. Please enter your PIN.");

            for (int i = 1; i <= maxLoginAttempts; i++)
            {
                Console.Write("\nPIN: ");

                string enteredPin = HidePin();

                if (enteredPin == user.Pin)
                {
                    Console.WriteLine($"\nPIN correct. \n\nWelcome {user.Name}!\nLogging you in. Please stand by...");
                    Thread.Sleep(4000);
                    return true;
                }
                else if (i < 2)
                {
                    Console.WriteLine($"\nInvalid PIN. Please try again. {maxLoginAttempts - i} tries left.");
                }
                if (i >= 2)
                {
                    Console.WriteLine($"\nInvalid PIN. Please try again. 1 try left.");
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
        private static bool Cooldown()
        {
            const int jailTime = 180; // 3 min

            Console.Clear();
            PrintLogo.PrintLockout();
            Console.WriteLine("You ran out of tries. Lockout timer initiated.");
            Console.CursorVisible = false;          
            for (int cooldown = jailTime; cooldown >= 0; cooldown--)
            {
                Console.SetCursorPosition(0, 10);
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
