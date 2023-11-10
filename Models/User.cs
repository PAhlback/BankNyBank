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

        internal static void OpenNewAccount(BankContext context, User user)
        {
            string newAccountname = Console.ReadLine();
            string salaryOrSavingAccount = Console.ReadLine();

            Account newAccount = new Account()
            {
                User = user,
                Name = newAccountname,
                Balance = 0,
                AccountType = salaryOrSavingAccount
            };
            context.Accounts.Add(newAccount);
            context.SaveChanges();

        }

    }
}

//public int Id { get; set; }
//public int UserId { get; set; }
//public string Name { get; set; }
//public double Balance { get; set; }
//public virtual User User { get; set; }
