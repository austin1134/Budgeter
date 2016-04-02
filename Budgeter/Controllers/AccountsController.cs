using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using CF_Budgeter.Models;
using Microsoft.AspNet.Identity;

namespace CF_Budgeter.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            var accounts = db.Accounts.Where(a => a.HouseholdId == currentUser);

            return View(accounts);
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Account account = db.Accounts.FirstOrDefault(x => x.Id == id);

            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            Household household = db.Households.FirstOrDefault(x => x.Id == account.HouseholdId);

            if (household.Id != user.HouseholdId)
            {
                throw new HttpException(401, "Unauthorized access");
            }
     
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Budget budget = new Budget();
            AccountDetailsViewModel accountDetailsViewModel = new AccountDetailsViewModel();
            //var account = await db.Accounts.FindAsync(id);
            var budget = await db.Budgets.FindAsync(id);

            //Passing my accounts and createTransactionViewModel properties to new accountDetailsViewModel in order
            //to get everything on my accounts page to be functional
            accountDetailsViewModel.Id = account.Id;
            accountDetailsViewModel.HouseholdId = account.HouseholdId;
            accountDetailsViewModel.Balance = account.Balance;
            accountDetailsViewModel.Name = account.Name;
            accountDetailsViewModel.ReconciledBalance = account.ReconciledBalance;
            accountDetailsViewModel.Transactions = db.Accounts.SelectMany(x => x.Transactions).Where(x => x.Account.HouseholdId == household.Id).ToList();/*account.Transactions;*/
            //accountDetailsViewModel.TransactionsThisMonth = accountDetailsViewModel.Transactions.Where(x => x.Date == DateTime.Now.Month);
            accountDetailsViewModel.TransactionCount = db.Accounts.SelectMany(x => x.Transactions).Where(x => x.Account.HouseholdId == household.Id).Count();
           /* accountDetailsViewModel.CurrentBudget = db.Budgets.FirstOrDefault(x => x.Household.Id)*/;

            if (budget != null)
            {
                accountDetailsViewModel.TotalBudgetAmount = budget.TotalBudgetAmount;
                accountDetailsViewModel.AvailableToSpend = budget.TotalBudgetAmount - account.Balance;
                accountDetailsViewModel.ProgressBar =
                Decimal.ToInt32(Decimal.Round(Decimal.Divide(100*account.Balance, budget.TotalBudgetAmount)));
            }

            accountDetailsViewModel.createTransactionViewModel = new CreateTransactionViewModel();
            accountDetailsViewModel.createTransactionViewModel.AccountId = account.Id;
            accountDetailsViewModel.createTransactionViewModel.Categories = new SelectList(db.Categories, "Id", "Name");
            if (accountDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            IEnumerable<Transaction> Transactions = accountDetailsViewModel.Transactions.OrderByDescending(t => t.Date);
            return View(accountDetailsViewModel);
        }

        // GET: Accounts/Create
        public ActionResult Create(int householdId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            Account account = new Account();
            var userHouseholds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Households;

            account.HouseholdId = user;
            //ViewBag.HouseholdId = new SelectList(userHouseholds, "Id", "Name", account.HouseholdId);
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdId,Balance,Name,ReconciledBalance")] Account account)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                    account.HouseholdId = user.HouseholdId;
                    //account.HouseholdId = ViewBag.HouseholdId;

                    db.Accounts.Add(account);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
            }
            //ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,HouseholdId,Balance,Name,ReconciledBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Account account = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(account);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
