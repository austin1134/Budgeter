using System;
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
        public decimal AverageTransaction { get; set; }
        public decimal AmountSpent { get; set; }
        public Household SelectedHousehold { get; set; }
        public Budget SelectedBudget { get; set; }
        

        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<BudgetItem> BudgetItems { get; set; }
        public IEnumerable<Household> YourHouseholds { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ChartItem> ChartData { get; set; }
    }

    public class ChartItem
    {
        public string Name { get; set; }
        public decimal AmountBudgeted { get; set; }
        public decimal AmountSpent { get; set; }
    }
}