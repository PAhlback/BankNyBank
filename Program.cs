using BankNyBank.Data;
using BankNyBank.Models;
using BankNyBank.Utilites;

namespace BankNyBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (BankContext context = new BankContext())
            {
                if (IsDatabaseEmpty(context))
                {
                    AddAdminUser(context);
                }
                MenuClass.MainMenu(context);
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
    }
}