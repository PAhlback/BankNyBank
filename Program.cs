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
                MenuClass.MainMenu(context);
            }
        }
    }
}