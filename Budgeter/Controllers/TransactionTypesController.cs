using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CF_Budgeter.Models;

namespace CF_Budgeter.Controllers
{
    [Authorize]
    public class TransactionTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TransactionTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.TransactionTypes.ToListAsync());
        }

        // GET: TransactionTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionType transactionType = await db.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return HttpNotFound();
            }
            return View(transactionType);
        }

        // GET: TransactionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] TransactionType transactionType)
        {
            if (ModelState.IsValid)
            {
                db.TransactionTypes.Add(transactionType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(transactionType);
        }

        // GET: TransactionTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionType transactionType = await db.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return HttpNotFound();
            }
            return View(transactionType);
        }

        // POST: TransactionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] TransactionType transactionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(transactionType);
        }

        // GET: TransactionTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionType transactionType = await db.TransactionTypes.FindAsync(id);
            if (transactionType == null)
            {
                return HttpNotFound();
            }
            return View(transactionType);
        }

        // POST: TransactionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TransactionType transactionType = await db.TransactionTypes.FindAsync(id);
            db.TransactionTypes.Remove(transactionType);
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
