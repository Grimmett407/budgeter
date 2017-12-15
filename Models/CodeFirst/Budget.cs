using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class Budget
    {
        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public double Total { get; set; }
        public int HouseholdId { get; set; }

        public virtual Household household { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }
}