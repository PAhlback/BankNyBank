using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites.AccountMethods
{
    internal class NewAccount
    {
        //Method for letting the user create a new account.
        public static void OpenNewAccount(BankContext context, User user)
        {
            bool currencyTypeSelected = false;
            bool accountTypeSelected = false;
            string newAccountName = "";
            string accountType = "";

            Console.Clear();
            Console.WriteLine("\nYou are about to create a new account.\n(Press any key to continue.)");
            Console.ReadKey();

            while (!accountTypeSelected)
            {
                Console.Clear();
                List<User> users = DbHelper.GetAllUsers(context);

                string pageHeader = "~~~~ What account type would you like it to be? ~~~~\n" +
                    "   ~~~~ 'Salary' or 'Savings' account? ~~~~";
                string[] menuOptions = { "Salary", "Savings" };

                int command = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);

                switch (command)
                {
                    case 1:
                        accountType = "salary";
                        accountTypeSelected = true;
                        break;
                    case 2:
                        accountType = "savings";
                        accountTypeSelected = true;
                        break;
                }
            }

            do
            {
                Console.WriteLine("\nChoose your new account name (minimum 3 characters): ");
                Console.Write("\nAccount name: ");
                newAccountName = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(newAccountName) || newAccountName.Length < 3)
                {
                    Console.Clear();
                    Console.WriteLine("\nAccount name cannot be empty or less than 3 characters. Please try again.\n");
                    Console.ReadKey();
                }
                else
                {
                    if (context.Accounts.Any(a => a.Name == newAccountName))
                    {
                        Console.Clear();
                        Console.WriteLine("Account name already exists. Please choose a different name.");
                        newAccountName = null;
                        Console.ReadKey();
                    }
                }
            }
            while (string.IsNullOrWhiteSpace(newAccountName) || newAccountName.Length < 3);

            string currency = "";

            while (!currencyTypeSelected)
            {
                Console.Clear();
                List<User> users = DbHelper.GetAllUsers(context);

                string pageHeader = "~~~~ What currency type would you like it to be? ~~~~\n" +
                    "   ~~~~ 'SEK', 'EUR' or 'USD' account? ~~~~";
                string[] menuOptions = { "SEK", "EUR", "USD" };

                int command = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);

                switch (command)
                {
                    case 1:
                        currency = "SEK";
                        currencyTypeSelected = true;
                        break;
                    case 2:
                        currency = "EUR";
                        currencyTypeSelected = true;
                        break;
                    case 3:
                        currency = "USD";
                        currencyTypeSelected = true;
                        break;

                }
            }

            Console.Clear();
            Console.WriteLine($"\nYour account name will be called {newAccountName} with currency {currency}.\n(Press any key to continue.)");
            Console.ReadKey();
            Console.WriteLine("Please wait.. Your account is being created...");
            Thread.Sleep(3000);

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
            Console.WriteLine("\nPress ENTER to return to main menu.");
            Console.ReadKey();
            Console.WriteLine("Returning to main menu...");
            Thread.Sleep(1000);
            return;
        }
    }
}
