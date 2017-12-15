using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class Household
    {
        public Household()
        {
            this.BankAccounts = new HashSet<BankAccount>();
            this.Transactions = new HashSet<Transaction>();
            this.Budgets = new HashSet<Budget>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string AuthorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double Total { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TotalBudget { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}