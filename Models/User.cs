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
            //User choice: Account name.
            Console.WriteLine("You are about to create a new account.");
            Console.Write("Choose your new account name: ");
            string newAccountname = Console.ReadLine();
            
            //User choice: Account type.
            Console.Write("'Salary' or 'savings' account?");
            Console.Write("Choose account type: ");
            string accountType = Console.ReadLine().ToLower();

            //Put While-loop here to return an invalid option.
            if (accountType == "salary" || accountType == "savings")
            {
                Account newAccount = new Account()
                {
                    User = user,
                    Name = newAccountname,
                    Balance = 0,
                    AccountType = accountType
                };
                context.Accounts.Add(newAccount);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("You did not choose a valid option.");
            }
        }
    }
}

//public int Id { get; set; }
//public int UserId { get; set; }
//public string Name { get; set; }
//public double Balance { get; set; }
//public virtual User User { get; set; }
