using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;
using CF_Budgeter.Models;
using Microsoft.AspNet.Identity;

namespace CF_Budgeter.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index(int id)
        {
            //var transactions = DateTime.FromOADate(30)
            var transactions = db.Transactions.Where(x => x.AccountId == x.Id).OrderByDescending(t => t.Date).ToList();

            //return View(await db.Transactions.OrderByDescending(t => t.Date).ToListAsync());
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Details(int accountId)
        {
            CreateTransactionViewModel createTransactionViewModel = new CreateTransactionViewModel();

            createTransactionViewModel.Date = DateTimeOffset.Now;
            createTransactionViewModel.AccountId = accountId;

            var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Household household = db.Households.FirstOrDefault(x => x.Id == user.HouseholdId);

            createTransactionViewModel.Categories = new SelectList(household.Categories.ToList(), "Id", "Name");

            return View(createTransactionViewModel);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequireHttps]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AccountId,UserId,Description,Date,Amount,TransactionTypeId,CategoryId,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                transaction.UserId = user.Id;
                if (user != null)

                {
                    var account = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountId);
                    account.Balance += transaction.Amount;

                    db.Transactions.Add(transaction);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", "Accounts", new {id = transaction.AccountId});
                }
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.Category = new SelectList(db.Categories, "Id", "Name", transaction.Category);

            return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId});
        }

        // GET: Transactions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AccountId,UserId,Description,Date,Amount,TransactionTypeId,CategoryId,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            transaction.UserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            var account = db.Accounts.FirstOrDefault(x => x.Id == transaction.AccountId);
            account.Balance -= transaction.Amount;
            db.Transactions.Remove(transaction);
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
