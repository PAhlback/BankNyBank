using BankNyBank.Data;
using BankNyBank.Models;
using BankNyBank.Utilites;
using BankNyBank.Utilites.AccountMethods;
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

        public static void UserMenu(BankContext context, User user)
        {
            string pageHeader = $"~~~~ Welcome, {user.Name} ~~~~";
            string[] menuOptions =
            {
                "View accounts and balance",
                "Transfer between accounts",
                "Withdraw",
                "Deposit",
                "Open new account",
                "Log out"
            };

            while (true)
            {
                int choice = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);
                Console.CursorVisible = true;

                switch (choice)
                {
                    case 1:
                        DbHelper.DisplayAccounts(context, user);
                        break;
                    case 2:
                        // Transfer between accounts
                        Transfer.TransferBetweenAccounts(context, user);
                        break;
                    case 3:
                        // Withdrawal
                        Withdraw.WithdrawMenu(context, user);
                        break;
                    case 4:
                        // Deposit
                        Deposit.DepositMenu(context, user);
                        break;
                    case 5:
                        // Open new account
                        NewAccount.OpenNewAccount(context, user);
                        break;
                    case 6:
                        Console.WriteLine("Logging out...");
                        Thread.Sleep(1000);
                        return;
                }

            }

        }

        
 
    }
}
