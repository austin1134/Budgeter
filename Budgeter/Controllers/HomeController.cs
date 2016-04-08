using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CF_Budgeter.Models;

namespace CF_Budgeter.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            DashBoardViewModel dashboard = new DashBoardViewModel();
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            db.Households.Find(user.HouseholdId);
            var userHouseholds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Households;
            var userBudgets = db.Budgets.Where(x => x.HouseholdId == user.HouseholdId).ToList();
            if (userHouseholds == null)
            {
                return RedirectToAction("Create", "Households");
            }

            dashboard.SelectedHousehold = db.Households.FirstOrDefault(x => x.Id == user.HouseholdId);
            dashboard.SelectedBudget = db.Budgets.FirstOrDefault(x => x.Id == user.BudgetId);

            ViewBag.SelectedHousehold = new SelectList(userHouseholds, "Id", "Name", dashboard.SelectedHousehold);
            ViewBag.SelectedBudget = new SelectList(userBudgets, "Id", "Name", dashboard.SelectedBudget);

            user.HouseholdId = dashboard.SelectedHousehold.Id;

            dashboard.Members = db.Users.Where(x => x.HouseholdId == dashboard.SelectedHousehold.Id);


            dashboard.TotalBudget = db.Budgets.Where(x => x.HouseholdId == user.HouseholdId).Select(b => b.TotalBudgetAmount).Sum();
            dashboard.Transactions = db.Transactions.Where(x => x.Account.HouseholdId == dashboard.SelectedHousehold.Id);
            dashboard.TotalSpent = dashboard.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).Select(x => x.Amount).Sum();
            dashboard.AvailableToSpend = dashboard.TotalBudget - dashboard.TotalSpent;
            if (dashboard.TotalSpent != 0 && dashboard.Transactions != null)
            {
                dashboard.AverageTransaction = decimal.Divide(dashboard.TotalSpent, dashboard.Transactions.Count());
            }
            dashboard.BudgetItems = db.BudgetItems.Where(x => x.BudgetId == user.BudgetId);

            dashboard.ChartData = db.BudgetItems.Where(c => c.Category.Households.Any(h => h.Id == user.HouseholdId))
                .ToList()
                .Select(c => CategoryToChartItem(c, user.HouseholdId, DateTime.Now));

            return View(dashboard);
        }


        // POST: Selected Household
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SelectedHousehold")] DashBoardViewModel dashBoard)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                {
                    user.HouseholdId = dashBoard.SelectedHousehold.Id;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Selected Budget
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "SelectedBudget")] DashBoardViewModel dashBoard)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                {
                    user.BudgetId = dashBoard.SelectedBudget.Id;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private ChartItem CategoryToChartItem(BudgetItem budgetItem, int householdId, DateTimeOffset date)
        {

            var transactions = db.Transactions.Where(t => t.Account.HouseholdId == householdId && t.Category.Id == budgetItem.CategoryId)
                                              //.Where(t => t.Amount < 0)
                                              .Where(t => t.Date.Month == date.Month && t.Date.Year == date.Year);
            var budgetItems = db.BudgetItems.Where(b => b.Budget.HouseholdId == householdId && b.Category.Id == budgetItem.CategoryId);

            if ((!transactions.Any() || transactions == null) && (!budgetItems.Any() || budgetItems == null))
                return null;

            return new ChartItem
            {
                Name = budgetItem.Category.Name,
                AmountSpent = transactions == null || !transactions.Any() ? 0 : transactions.Sum(t => t.Amount),
                AmountBudgeted = (decimal) (budgetItems == null || !budgetItems.Any() ? 0 : budgetItems.Sum(b => b.Amount))
            };
        }
    }
}