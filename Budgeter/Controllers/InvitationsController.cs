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
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNet.Identity;
//using SendGrid;

namespace CF_Budgeter.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        public async Task<ActionResult> Index()
        {

            var user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var invitations = db.Invitations.Where(x => x.ToEmail == user.Email).Include(i => i.Household);
            return View(await invitations.ToListAsync());
        }

        public ActionResult Create([Bind(Include = "ToEmail, HouseholdId")] Invitations invitation)
        {
            if (ModelState.IsValid)
            {
                //Build and store the invitation object
                invitation.UserId = User.Identity.GetUserId();
                invitation.JoinCode = Guid.NewGuid();
                db.Invitations.Add(invitation);
                db.SaveChanges();


                try
                {
                    //Build the mail message
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.To.Add(new MailAddress(invitation.ToEmail, "To"));
                    mailMsg.From = new MailAddress(User.Identity.Name, "From");
                    mailMsg.Subject = "CF-Budgeter: Invitation to Join Household";
                    var callbackUrl = Url.Action("ExistingUserJoin", "Households", new {JoinCode = invitation.JoinCode},
                        protocol: Request.Url.Scheme);
                    StringBuilder str = new StringBuilder();
                    str.Append(@"<p>");
                    str.Append(User.Identity.Name);
                    str.Append(
                        " would like you to join their household in the CF-Budgeter household budgeting system.</p><p><a href='");
                    str.Append(callbackUrl);
                    str.Append("'>Join</a></p>");

                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(str.ToString(), null,
                        MediaTypeNames.Text.Html));

                    //Init SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                    var sendGridCredentials = db.SendGridCredentials.First();
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                        sendGridCredentials.Username, sendGridCredentials.Password);
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);

                    //If successful, we wil redirect back to the Dashboard. If not, an exception will be thrown.
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            var user = (ApplicationUser)Session["User"];
            //ViewBag.HouseholdId = new SelectList(user.Households.OrderBy(h => h.Name), "Id", "Name",
            //    user.HouseholdId());

            return View(invitation);
        }

        //public Task SendAsync(IdentityMessage message)
        //{
        //    invitation.UserId = User.Identity.GetUserId();
            

        //    var username = db.SendGridCredentials.First().Username;
        //    var password = db.SendGridCredentials.First().Password;

        //    SendGridMessage sendGridMessage = new SendGridMessage();
        //    sendGridMessage.Subject = message.Subject;
        //    sendGridMessage.Html = message.Body;
        //    sendGridMessage.From = new MailAddress(User.Identity.Name);
        //    sendGridMessage.AddTo(message.Destination);
        //    var credential = new NetworkCredential(username, password); 
        //    var transportweb = new Web(credential);
        //    transportweb.DeliverAsync(sendGridMessage);
        //    return Task.FromResult(0);
        //}

        //public ActionResult BuildInvitation(Invitations invitations)
        //{
        //    var myMessage = new IdentityMessage();
        //    string url = Url.Action("Login", "Account");
        //    myMessage.Subject = "Invitation To Join Austins Budget Tracker";
        //    myMessage.Body = "Hello!" + " " + @User.Identity.Name + " " + "has invited you to join their household on Austins Budget Tracker. Click <a href=\"" + url + "\"> </a> to accept their invitation" ;
        //    myMessage.Destination = invitations.ToEmail;
        //    SendAsync(myMessage);
        //    return RedirectToAction("Details", "Households", new {id = invitations.HouseholdId});
        //}

        // GET: Invitations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitations invitations = await db.Invitations.FindAsync(id);
            if (invitations == null)
            {
                return HttpNotFound();
            }
            return View(invitations);
        }

        // GET: Invitations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
        //    var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        //    return View(currentUser.Households);
        //}

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,ToEmail,UserId,HouseholdId,JoinCode,Joined")] Invitations invitations)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Invitations.Add(invitations);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitations.HouseholdId);
        //    return View(invitations);
        //}

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
