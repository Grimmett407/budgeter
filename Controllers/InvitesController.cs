using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using budgeter.Models;
using budgeter.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using budgeter.Models.Helpers;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;

namespace budgeter.Controllers
{
    public class InvitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invites/Create
        public ActionResult Create(int id)
        {
            Household household = db.Households.Find(id);
            ViewBag.HouseholdName = household.Title;
            ViewBag.HouseholdId = id;
            return View();
        }

        // POST: Invites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdId,Created,SentById,ToEmail,Secret")] Invite invite, int id)
        {
            if (ModelState.IsValid)
            {
                var houseId = db.Households.Find(id);
                invite.HouseholdId = houseId.Id;
                invite.Secret = Utility.GenerateSecretCode();
                string userId = User.Identity.GetUserId();
                invite.SentById = userId;
                invite.Created = DateTimeOffset.Now;
                var email = new MailMessage(ConfigurationManager.AppSettings["username"], invite.ToEmail)
                {
                    Subject = "Invite to BudgetHouse!",
                    Body = "<p>You've been invited to join a household on the BudgetHouse app. </p><p>Select \"Join\" on log in and input the code: " + invite.Secret + ". </p>Or, <a href=\"" + 
                    Url.Action("InviteRegister", "Account", new { householdId = invite.HouseholdId, secret = invite.Secret }, Request.Url.Scheme) + 
                    "\">Click here</a> to join.",
                    IsBodyHtml = true
                };
                db.Invites.Add(invite);
                db.SaveChanges();

                var svc = new PersonalEmail();
                await svc.SendAsync(email);

                return RedirectToAction("Details", "Households", new { id = id });
            }

            ViewBag.HouseholdId = id;
            return View(invite);
        }

        

        // GET: Invites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // POST: Invites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invite invite = db.Invites.Find(id);
            db.Invites.Remove(invite);
            db.SaveChanges();
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




        //// GET: Invites
        //public ActionResult Index()
        //{
        //    var invites = db.Invites.Include(i => i.Household).Include(i => i.SentBy);
        //    return View(invites.ToList());
        //}

        //// GET: Invites/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Invite invite = db.Invites.Find(id);
        //    if (invite == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(invite);
        //}

        //// GET: Invites/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Invite invite = db.Invites.Find(id);
        //    if (invite == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Title", invite.HouseholdId);
        //    ViewBag.SentById = new SelectList(db.Users, "Id", "FirstName", invite.SentById);
        //    return View(invite);
        //}

        //// POST: Invites/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,HouseholdId,Created,SentById,ToEmail,Secret")] Invite invite)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(invite).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Title", invite.HouseholdId);
        //    ViewBag.SentById = new SelectList(db.Users, "Id", "FirstName", invite.SentById);
        //    return View(invite);
        //}
    }
}
