using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public bool Deleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual Budget Budget { get; set; }
    }
}