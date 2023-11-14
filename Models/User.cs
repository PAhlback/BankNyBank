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

            do
            {
                Console.Clear();
                Console.WriteLine("\nYou are about to create a new account.\n(Press key to continue.)");
                Console.ReadKey();
                Console.Write("\nChoose your new account name: ");
                newAccountName = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(newAccountName) )
                {
                    Console.WriteLine("\nAccount name cannot be empty. Please try again.\n");
                }
            } while (string.IsNullOrWhiteSpace(newAccountName));

            do
            {
                Console.Clear();
                Console.WriteLine($"\nYour account name is {newAccountName}.\n(Press key to continue.)");
                Console.ReadKey();
                Console.WriteLine("\nWhat account type would you like it to be?\n'Salary' or 'savings' account?");
                Console.Write("\nAccount type: ");
                accountType = Console.ReadLine().ToLower();

                if (accountType != "salary" && accountType != "savings")
                {
                    Console.WriteLine("You did not choose a valid option. Please try again.");
                }
            } while (accountType != "salary" && accountType != "savings");

            Account newAccount = new Account()
            {
                UserId = user.Id,
                Name = newAccountName,
                Balance = 0,
                AccountType = accountType,
            };
            context.Accounts.Add(newAccount);
            context.SaveChanges();

            Console.WriteLine("\n(Press key to continue)");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("\nYou have successfully created a new account.");
            Console.WriteLine($"\tNew Account Information:\n\tName: {newAccountName}\n\tType: {accountType}.");
            Console.ReadKey();

        }
    }
}
