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
            string msg = "~~~~ Select account to transfer from ~~~~";
            Account acc1 = GetSingleAccountFromUsersAccounts(context, user, msg);
            msg = "~~~~ Select account to transfer to ~~~~";
            Account acc2 = GetSingleAccountFromUsersAccounts(context, user, msg);
            double transferAmount = 0;

            // Takes the amount to be transfered as a string and checks that it only contains
            // numbers, and that it is not higher than the current balance of the account.
            while (true)
            {
                bool isParsed = false;
                Console.Write("Enter amount to transfer: ");
                string input = Console.ReadLine();
                if (input.All(char.IsDigit))
                {
                    transferAmount = double.Parse(input);
                    isParsed = true;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Transfer amount can only contain numbers.");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                // Does a check to see if the amount being transfered is a positive number,
                // and is within the range of the account being transfered from.
                if (isParsed)
                {
                    if (transferAmount <= 0 || transferAmount > acc1.Balance)
                    {
                        Console.WriteLine("\nInvalid amount. Please enter amount within balance of account.");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            // "Transfers" between the accounts using the withdraw and deposit methods.
            Console.WriteLine("Transfering...");
            DbHelper.WithdrawFromAccount(context, acc1, transferAmount);
            // Converts the transfer amount to the correct currency of the recieving account.
            transferAmount = ConvertCurrency(acc1, acc2, transferAmount);
            DbHelper.DepositToAccount(context, acc2, transferAmount);

            Console.WriteLine("Transfer complete!");

            // Display the new balance of the accounts transfered between
            Console.WriteLine("\nNew account balances:\n");
            DbHelper.PrintOneAccount(acc1.Name, acc1.Balance.ToString("N2"), acc1.Currency);
            DbHelper.PrintOneAccount(acc2.Name, acc2.Balance.ToString("N2"), acc2.Currency);

            //Console.WriteLine("==============================================");
            //Console.WriteLine($"Account: {acc1.Name}");
            //Console.WriteLine($"Balance: {acc1.Balance} {acc1.Currency}");
            //Console.WriteLine("==============================================\n");
            //Console.WriteLine("==============================================");
            //Console.WriteLine($"Account: {acc2.Name}");
            //Console.WriteLine($"Balance: {acc2.Balance} {acc2.Currency}");
            //Console.WriteLine("==============================================\n");

            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
        }

        static Account GetSingleAccountFromUsersAccounts(BankContext context, User user, string msg)
        {
            string pageHeader = msg;

            var accountList = context.Accounts
                .Where(u => u.User.Id == user.Id)
                .ToList();

            string[] menuOptions = new string[accountList.Count];

            for (int i = 0; i < accountList.Count; i++)
            {
                menuOptions[i] = accountList[i].Name;
            }

            int choice = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);

            return accountList[choice - 1];
        }

        // Method for converting transfer amount between accounts with different currencies.
        private static double ConvertCurrency(Account a, Account b, double transferAmount)
        {
            // Approximate rates from november 19. 
            // 1 USD = 10.5 SEK
            // 1 EUR = 11.5 SEK
            // 1 EUR = 1,1 USD
            const double SeEu = 0.087;
            const double SeUs = 0.1;
            const double EuSe = 11.5;
            const double EuUs = 0.925;
            const double UsEu = 1.09;
            const double UsSe = 10.5;
            
            //if (a.Currency != b.Currency)
            //{
            //    // Alot of if-else chained together. Could maybe change to an if-statement to check currency
            //    // of "a", and then an if inside to check currency of "b", which might perform fewer checks to
            //    // get to the end. Goes from a max of 6 checks to a max of 5 checks, so probably not worth it.
            //    if (a.Currency == "SEK" &&  b.Currency == "EUR") return transferAmount *= SeEu;
            //    else if (a.Currency == "SEK" && b.Currency == "USD") return transferAmount *= SeUs;
            //    else if (a.Currency == "EUR" && b.Currency == "SEK") return transferAmount *= EuSe;
            //    else if (a.Currency == "EUR" && b.Currency == "USD") return transferAmount *= EuUs;
            //    else if (a.Currency == "USD" && b.Currency == "SEK") return transferAmount *= UsSe;
            //    else if (a.Currency == "USD" && b.Currency == "EUR") return transferAmount *= UsEu;
            //}

            if (a.Currency == b.Currency)
            {
                return transferAmount;
            }

            switch (a.Currency)
            {
                case "SEK":
                    if (b.Currency == "EUR") return transferAmount *= SeEu;
                    else return transferAmount *= SeUs;
                case "EUR":
                    if (b.Currency == "SEK") return transferAmount *= EuSe;
                    else return transferAmount *= EuUs;
                case "USD":
                    if (b.Currency == "SEK") return transferAmount *= UsSe;
                    else return transferAmount *= UsEu;
            }

            // Needs to have this or VS gives the error "Not all code path returns a value"
            return transferAmount;
        }
    }
}
