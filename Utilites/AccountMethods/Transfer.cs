using BankNyBank.Data;
using BankNyBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNyBank.Utilites.AccountMethods
{
    internal class Transfer
    {
        public static void TransferBetweenAccounts(BankContext context, User user)
        {
            // Check if user wants to transfer to another user, and declares some variables that 
            // are used later (and which needs to be used outside of where they are given values.
            string pageHeader = "Transfer to other user?";
            string[] menuOptions = { "Yes", "No" };

            int toOtherUser = MenuManager.DisplayAndGetMenuChoice(pageHeader, menuOptions);

            User foreignUser = new User();
            Account acc2 = new Account();

            // This while loop gets an account from another user. This could probably be it's own method, which
            // returns the account, instead of sitting right here.
            while (toOtherUser == 1)
            {
                // First declares some variables used to navigate the if statements in the while loop. Then goes through the 
                // if statements. First it looks for the user. Then, when a user is found, gets an account from the user.
                // If a user or account is not found, it gives the option of Trying again, exit or transfer within the users
                // own accounts.
                bool getUser = true;
                string otherUserName = string.Empty;
                int choice = 0;
                bool getAccount = false;

                string[] menuOptions2 = { "Try again", "Transfer between own accounts", "Exit" };

                if (getUser)
                {
                    Console.Write("Enter user name to transfer to: ");
                    otherUserName = Console.ReadLine();
                    foreignUser = UserAuthentication.FindUserByName(context, otherUserName);
                }

                // Checks to make sure a user was found.
                if (foreignUser == null) 
                {
                    string pageHeader1 = $"Could not find user with name {otherUserName}";
                    choice = MenuManager.DisplayAndGetMenuChoice (pageHeader1, menuOptions2);
                    getAccount = false;
                }
                else
                {
                    getUser = false;
                    getAccount = true;
                }

                if (getAccount)
                {
                    Console.Write("Enter account name to transfer to: ");
                    string accountNameForeignUser = Console.ReadLine();
                    List<Account> foreignAccounts = context.Accounts
                        .Where(a => a.User.Name == foreignUser.Name).ToList();
                    Account foundAccount = foreignAccounts.Find(a => a.Name == accountNameForeignUser);

                    // Checks to make sure an account from the foreign user was found.
                    if (foundAccount.Name == null)
                    {
                        string pageHeader2 = $"Could not find account with name {accountNameForeignUser}";
                        choice = MenuManager.DisplayAndGetMenuChoice(pageHeader2, menuOptions2);
                    }
                    else
                    {
                        acc2 = foundAccount;
                        toOtherUser = 0;
                        Console.WriteLine("Account found!");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                    }
                }
                switch (choice)
                {
                    case 1:
                        break;
                    case 2:
                        toOtherUser = 0;
                        break;
                    case 3:
                        Console.Clear();
                        return;
                }
                Console.Clear();
            }

            string msg = "~~~~ Select account to transfer from ~~~~";
            Account acc1 = GetSingleAccountFromUsersAccounts(context, user, msg);

            if (toOtherUser == 2)
            {
                msg = "~~~~ Select account to transfer to ~~~~";
                acc2 = GetSingleAccountFromUsersAccounts(context, user, msg);
            }

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
            DbHelper.PrintOneAccount(acc1.Name, acc1.AccountType, acc1.Balance.ToString("N2"), acc1.Currency);
            DbHelper.PrintOneAccount(acc2.Name, acc2.AccountType, acc2.Balance.ToString("N2"), acc2.Currency);

            Console.WriteLine("Press ENTER to return to menu");
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
