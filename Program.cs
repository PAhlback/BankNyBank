using BankNyBank.Data;
using BankNyBank.Models;
using BankNyBank.Utilites;

namespace BankNyBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "BankNyBank Corporation";
            Console.ForegroundColor = ConsoleColor.Yellow;
            using (BankContext context = new BankContext())
            {
                if (IsDatabaseEmpty(context))
                {
                    AddAdminUser(context);
                }
                WelcomeScreen(context);
            }
        }
        static bool IsDatabaseEmpty(BankContext context)
        {
            return context.Users.Count() == 0;
        }

        static User AddAdminUser(BankContext context)
        {
            // Add admin user with pin "1234"
            User adminUser = new User { Name = "admin", Pin = "1234" };
            context.Users.Add(adminUser);
            context.SaveChanges();

            Console.WriteLine("Admin user added to the database on finding the database empty.\n" +
                "Name = admin\n" +
                "Pin = 1234");

            return adminUser;
        }

        public static void WelcomeScreen(BankContext context)
        {
            {
                // Add while loop
                PrintLogo.PrintBankNyBank();
                Console.WriteLine("Welcome, valued customer!\n" +
                    "______________________________________\n" +
                    "\nPlease provide your login credentials:\n");

                // Check user login method
                User user = UserAuthentication.CheckLogIn(context);

                // Call 'AdminMenu' or 'UserMenu' depending on username
                MenuManager.DisplayMenu(context, user);
            }
        }
    }
}