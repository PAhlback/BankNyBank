using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Models
{
    // Can use a class "transaction" to store tansactions.
    internal class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string AccountType { get; set; }

        public virtual User User { get; set; }


        //Ewwwww Vet inte om jag gillar.
        //
        //void AccountTypeChecker()
        //{
        //    Console.Write("Would you like to make it a salary or savings account? ");
        //    string accountType = Console.ReadLine();
        //    AccountType = accountType;

        //    while (accountType != "salary" && accountType != "savings")
        //    {
        //        Console.WriteLine("Invalid choice for account, please choose 'salary' or 'savings'.");
        //        return;
        //    }
        //}

    }
}
