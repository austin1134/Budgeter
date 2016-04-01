using System;
using System.Collections.Generic;
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

            var userHouseholds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Households;
            if (userHouseholds == null)
            {
                return RedirectToAction("Create", "Households");
            }

            ViewBag.SelectedHousehold = new SelectList(userHouseholds, "Id", "Name", dashboard.SelectedHousehold);

            //if (ViewBag.SelectedHousehold == null)
            //{
            //    Sele == userHouseholds.FirstOrDefault();
            //}

            //dashboard.SelectedHousehold = ViewBag.HouseholdId;

            dashboard.Members = db.Users.Where(x => x.HouseholdId == dashboard.SelectedHousehold);

            dashboard.TotalBudget = db.Budgets.Where(x => x.HouseholdId == dashboard.SelectedHousehold).Select(b => b.TotalBudgetAmount).Sum();
            dashboard.Transactions = db.Transactions.Where(x => x.Account.HouseholdId == dashboard.SelectedHousehold);
            dashboard.TotalSpent = dashboard.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).Select(x => x.Amount).Sum();
            dashboard.AvailableToSpend = dashboard.TotalBudget - dashboard.TotalSpent;

            return View();
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