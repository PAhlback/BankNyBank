using BankNyBank.Data;
using BankNyBank.Utilites;
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
        public string Currency { get; set; }
        public string AccountType { get; set; }

        public virtual User User { get; set; }

        public static void TransferBetweenAccounts(BankContext context, User user)
        {

            Account acc1 = GetAccount(context, user);
            Account acc2 = GetAccount(context, user);

            Console.Write("Choose amount to transfer: ");

            int trAmount = int.Parse(Console.ReadLine());

            acc1.Balance -= trAmount;
            acc2.Balance += trAmount;
            Console.WriteLine("Transfering...");
            context.SaveChanges();

            Console.WriteLine("Transfer complete!");
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
        }

        static Account GetAccount(BankContext context, User user)
        {
            string pageHeader = "Select account";

            var accountList = context.Accounts
                .Where(u => u.User.Id == user.Id)
                .ToList();

            string[] menuOptions = new string[accountList.Count];

            for (int i = 0; i < accountList.Count; i++)
            {
                menuOptions[i] = accountList[i].Name;
            }

            int choice = MenuClass.DisplayAndGetMenuChoice(pageHeader, menuOptions);

            Account account = accountList[choice - 1];

            return account;
        }
    }
}
