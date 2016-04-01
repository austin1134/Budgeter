using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CF_Budgeter.Models
{
    public class HouseholdDetailsViewModel
    {
        public string Name { get; set; }

        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<BudgetItem> BudgetItems { get; set; } 
        public IEnumerable<ApplicationUser> Members { get; set; }

    }
}