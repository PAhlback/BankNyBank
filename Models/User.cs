using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }

        //Method for letting the user create a new account.
        public static void OpenNewAccount(BankContext context, User user)
        {
            string newAccountName = "";
            string accountType = "";

            Console.Clear();
            Console.WriteLine("\nYou are about to create a new account.\n(Press any key to continue.)");
            Console.ReadKey();

            do
            {
                Console.Clear();
                Console.WriteLine("\nWhat account type would you like it to be?\n'Salary' or 'savings' account?");
                Console.Write("\nAccount type: ");
                accountType = Console.ReadLine().ToLower();

                if (accountType != "salary" && accountType != "savings")
                {
                    Console.WriteLine("\nYou did not choose a valid option. Please try again.\n");
                }
            } while (accountType != "salary" && accountType != "savings");

            do
            {
                Console.WriteLine("\nChoose your new account name (minimum 3 characters): ");
                Console.Write("\nAccount name: ");
                newAccountName = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(newAccountName) || newAccountName.Length < 3)
                {
                    Console.Clear();
                    Console.WriteLine("\nAccount name cannot be empty or less than 3 characters. Please try again.\n");
                }
                else
                {
                    // Check if the desired account name already exists in the database
                    if (context.Accounts.Any(a => a.Name == newAccountName))
                    {
                        Console.Clear();
                        Console.WriteLine("Account name already exists. Please choose a different name.");
                        newAccountName = null; // Set to null to repeat the loop
                    }
                }
            } while (string.IsNullOrWhiteSpace(newAccountName) || newAccountName.Length < 3);


            /* Enforce the user to choose a currency for the account
               Prompts the user until valid input is provided */
            string currency;
            do
            {
                Console.WriteLine("Choose a currency for the account (SEK, EUR, USD):");
                currency = Console.ReadLine().ToUpper();

                if (currency != "SEK" && currency != "EUR" && currency != "USD")
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input.");
                }
            } while (currency != "SEK" && currency != "EUR" && currency != "USD");


            Console.Clear();
            Console.WriteLine($"\nYour account name will be called {newAccountName} with currency {currency}.\n(Press any key to continue.)");
            Console.ReadKey();

            Account newAccount = new Account
            {
                UserId = user.Id,
                Name = newAccountName,
                Balance = 0,
                AccountType = accountType,
                Currency = currency
            };
            context.Accounts.Add(newAccount);
            context.SaveChanges();

            Console.Clear();
            Console.WriteLine("\nYou have successfully created a new account.\n");
            Console.WriteLine($"\tNew Account Information:\n\t~~~~~~~~~~~~~~~~~~~~~~~~\n\tName: \t\t{newAccountName}\n\tType: \t\t{accountType}\n\tCurrency: \t{currency}");
            Console.ReadKey();
        }
    }
}
