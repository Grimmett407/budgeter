using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public DateTimeOffset CreatedDate { get; set; }
        public double Amount { get; set; }
        public int BankAccountId { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string AuthorId { get; set; }
        public bool Deleted { get; set; }
        public string Payee { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual Type Type { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}