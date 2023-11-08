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
                //User admin = new User()
                //{
                //    Name = "admin",
                //    Pin = "1234"
                //};
                //context.Users.Add(admin);
                //context.SaveChanges();
                //Console.ReadLine();
                MenuClass.MainMenu(context);
            }

        }
    }
}