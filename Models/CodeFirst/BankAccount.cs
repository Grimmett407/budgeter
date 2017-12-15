using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class BankAccount
    {
        public BankAccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public double Balance { get; set; }
        public int HouseholdId { get; set; }
        public bool Deleted { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}