using budgeter.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace budgeter.Models.Helpers
{
    public class HouseholdHelper
    {
        public Household household { get; set; }
        public List<Transaction> MyTransactions { get; set; }
        public List<BankAccount> MyBankAccounts { get; set; }
        public List<BudgetItem> MyBudgetItems { get; set; }
        public int[] MyCategories { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double[] CategoryTotals { get; set; }
        public int CategoryCount { get; set; }
    }
}