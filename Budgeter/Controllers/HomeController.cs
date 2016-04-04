﻿using System;
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
            if (userHouseholds == null)
            {
                return RedirectToAction("Create", "Households");
            }
            //dashboard.UserHousehold = db.Households.Where(x => x.Id == user.HouseholdId)

            ViewBag.SelectedHousehold = new SelectList(userHouseholds, "Id", "Name", dashboard.SelectedHousehold/*dashboard.SelectedHousehold*/);

            user.HouseholdId = dashboard.SelectedHousehold;

            dashboard.Members = db.Users.Where(x => x.HouseholdId == dashboard.SelectedHousehold);

            //dashboard.TotalBudget = db.Budgets.Where(x => x.HouseholdId == dashboard.SelectedHousehold).Select(b => b.TotalBudgetAmount).Sum();
            dashboard.Transactions = db.Transactions.Where(x => x.Account.HouseholdId == dashboard.SelectedHousehold);
            dashboard.TotalSpent = dashboard.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).Select(x => x.Amount).Sum();
            dashboard.AvailableToSpend = dashboard.TotalBudget - dashboard.TotalSpent;

            return View(dashboard);
        }

        // POST: Selected Household
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SelectedHousehold")] DashBoardViewModel dashBoard)
        {
            var userHouseholds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Households;
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                {
                    user.HouseholdId = dashBoard.SelectedHousehold;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.SelectedHousehold = new SelectList(userHouseholds, "Id", "UserName", dashBoard.SelectedHousehold);
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