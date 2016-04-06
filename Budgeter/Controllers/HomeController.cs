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

            dashboard.BudgetItems = db.BudgetItems.Where(x => x.BudgetId == user.BudgetId);


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














        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}