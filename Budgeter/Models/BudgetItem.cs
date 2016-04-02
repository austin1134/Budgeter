using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CF_Budgeter.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public double Amount { get; set; }
        public decimal AmountSpent { get; set; }
        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }
    }
}