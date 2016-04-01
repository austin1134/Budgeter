﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CF_Budgeter.Models
{
    public class DashBoardViewModel
    {
        public decimal TotalBudget { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AvailableToSpend { get; set; }
        public int SelectedHousehold { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<BudgetItem> BudgetItems { get; set; }
        public IEnumerable<Household> YourHouseholds { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
    }
}