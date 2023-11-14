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
    }
}
