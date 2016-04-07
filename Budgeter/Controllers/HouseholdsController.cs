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
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Net.Mail;

namespace CF_Budgeter.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Households;
            return View(user);
        }

        // GET: Households/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ApplicationUser user = db.Users.Single(x => x.UserName == User.Identity.Name);
            var household = await db.Households.FindAsync(id);
            //Household household = db.Households.SingleOrDefault(x => x.Id == id);

            if (!household.Members.Contains(user))
            {
                throw new HttpException(401, "Unauthorized access");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Join(string JoinCode)
        {
            //set up the confirmation page so the user can confim he wants to change households
            //might also need a view model for this page
            var invitation = await db.Invitations.FirstOrDefaultAsync(i => i.JoinCode.ToString() == JoinCode);

            if (invitation != null)
            {
                var model = new RegisterViewModel
                {
                    HouseholdId = invitation.Household.Id,
                    HouseholdName = invitation.Household.Name,
                    Email = invitation.ToEmail
                };
                var user = db.Users.FirstOrDefault(u => u.Email == invitation.ToEmail);
                if (user != null)
                {
                    //then user exists / populate user info
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new {message = "Unauthorized"});
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExistingUserJoin(string JoinCode)
        {

            var invitation = await db.Invitations.FirstOrDefaultAsync(i => i.JoinCode.ToString() == JoinCode);
            var user = db.Users.FirstOrDefault(u => u.Email == invitation.ToEmail);
            Household newHousehold = await db.Households.FindAsync(invitation.HouseholdId);

            if (User != null)
            {
                user.Households.Add(newHousehold);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Households");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { message = "Unauthorized" });
            }
        }


        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {

                var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                user.Households.Add(household);
                await db.SaveChangesAsync();

                user.HouseholdId = household.Id;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = await db.Households.FindAsync(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = await db.Households.FindAsync(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Household household = await db.Households.FindAsync(id);
            db.Households.Remove(household);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Household/Leave
        public ActionResult Leave(int? id)
        {
            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Household household = db.Households.FirstOrDefault(x => x.Id == id);

            if (!household.Members.Contains(user))
            {
                throw new HttpException(401, "Unauthorized access");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Household/Leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveConfirmed(int id)
        {
            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            Household household = db.Households.FirstOrDefault(x => x.Id == id);

            if (!household.Members.Contains(user))
            {
                throw new HttpException(401, "Unauthorized access");
            }

            user.Households.Remove(household);
            db.SaveChanges();

            if (household.Members.Count == 0)
            {
                db.Households.Remove(household);
            }

            return RedirectToAction("Index", "Home");
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
