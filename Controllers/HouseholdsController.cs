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
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using budgeter.Models.Helpers;
using static budgeter.Models.Helpers.HouseholdUserHelper;

namespace budgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
           
            var other = db.Households.Where(h => h.Users.Any(u => (u.Id.Contains(user.Id))));
            //var role = db.Users.Where(u => u.Households.Any(h => (other.Any(o => o.))));
            //var userhouse = db.Households.Where(h => user.Id.Contains(h.AuthorId || other));
            return View(other.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
             HouseholdHelper model = new HouseholdHelper();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.household = db.Households.Find(id);
            if (model.household == null)
            {
                return HttpNotFound();
            }

            foreach (var bankAccount in model.household.BankAccounts)
            {
                if (bankAccount.Deleted != true)
                {
                    if (bankAccount.Balance <= 0)
                    {
                        ViewBag.OverdraftError = "You are in danger of overdrawing your account!";
                    }

                    var balance = bankAccount.Balance;
                    model.household.Total += balance;
                    //total = total + balance;
                }
            }

            model.household.TotalBudget = 0;

            foreach (var budget in model.household.Budgets)
            {
                foreach (var item in budget.BudgetItems)
                {
                    var amount = item.Amount;
                    model.household.TotalBudget += amount;
                }
            }

            int catTotal = 0;

            foreach (var category in db.Categories)
            {
                catTotal++;
            }

            int catCount = 0;
            model.MyCategories = new int[catTotal - 1];

            foreach (var budget in model.household.Budgets)
            {
                foreach (var item in budget.BudgetItems)
                {
                    model.MyCategories[catCount] = item.Category.Id;
                    catCount++;
                }
            }

            model.CategoryCount = catCount;

            //double categoryTotal = 0;
            int transCount = 0;
            model.CategoryTotals = new double[catTotal - 1];

            foreach (var category in model.MyCategories)
            {
                foreach (var account in model.household.BankAccounts)
                {
                    foreach (var transaction in account.Transactions)
                    {
                        if (transaction.CategoryId == category)
                        {
                            //model.MyTransactions.Add(transaction);
                            //model.CategoryTotals += transaction.Amount;
                            model.CategoryTotals[transCount] += transaction.Amount;
                        }
                    }
                }
                transCount++;
            }

            db.SaveChanges();
            return View(model);
        }


        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Household household)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                household.CreatedDate = System.DateTimeOffset.Now;
                household.AuthorId = User.Identity.GetUserId();
                db.Households.Add(household);

                HouseholdUsersHelper helper = new HouseholdUsersHelper(db);
                helper.AddUserToHousehold(household.Id, user.Id);

                Budget budget = new Budget
                {
                    HouseholdId = household.Id,
                    household = db.Households.Find(household.Id),
                    Title = household.Title + "'s Budget",
                    Total = 0
                };
                db.Budgets.Add(budget);

                db.SaveChanges();
                return RedirectToAction("Details",  new { id = household.Id});
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }


        // GET: Households/Assign/5
        public ActionResult Assign(int id)
        {
            var household = db.Households.Find(id);
            HouseholdUsersHelper helper = new HouseholdUsersHelper(db);
            var model = new AssignUsersViewModel();

            model.Household = household;
            model.SelectedUsers = helper.ListAssignedUsers(id).ToArray();
            model.Users = new MultiSelectList(model.SelectedUsers.Where(u => (u.DisplayName != "N/A" && u.DisplayName != "(Remove Assigned User)")).OrderBy(u => u.FirstName), "Id", "DisplayName", model.SelectedUsers);
            //model.Users = new MultiSelectList(db.Users.Where(u => (u.DisplayName != "N/A" && u.DisplayName != "(Remove Assigned User)")).OrderBy(u => u.FirstName), "Id", "DisplayName", model.SelectedUsers);

            return View(model);
        }

        // POST: Households/Assign/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Assign(AssignUsersViewModel model)
        {
            var household = db.Households.Find(model.Household.Id);
            HouseholdUsersHelper helper = new HouseholdUsersHelper(db);

            foreach (var user in db.Users.Select(r => r.Id).ToList())
            {
                if (model.SelectedUsers != null)
                {
                    foreach (var item in model.SelectedUsers)
                    {
                        helper.RemoveUserFromHousehold(household.Id, item.Id);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }


        // GET: Households/Leave/5
        public ActionResult Leave(int? id)
        {
            Household household = db.Households.Find(id);
            ViewBag.HouseholdName = household.Title;
            ViewBag.HouseholdId = id;
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

        // POST: Households/Leave/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(int id)
        {
            var user = User.Identity.GetUserId();
            HouseholdUsersHelper helper = new HouseholdUsersHelper(db);
            helper.RemoveUserFromHousehold(id, user);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //invite members
        //public async Task<ActionResult> Invite(Invite invite, int? id)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var houseId = db.Households.Find(id);
        //        invite.HouseholdId = Convert.ToInt32(houseId);
        //        invite.Secret = Utility.GenerateSecretCode();
        //        string userId = User.Identity.GetUserId();
        //        invite.SentById = userId;
        //        invite.Created = DateTimeOffset.Now;
        //        var email = new MailMessage(ConfigurationManager.AppSettings["username"], invite.ToEmail)
        //        {
        //            Subject = "Invite to BudgetHouse!",
        //            Body = "<p>You've been invited to join a household on the BudgetHouse app. </p><p>Select \"Join\" on log in and input the code: " + invite.Secret + ". </p>Or, <a href=\"" + Url.Action("InviteRegister", "Account", new { householdId = invite.HouseholdId, secret = invite.Secret }, Request.Url.Scheme) + "\">Click here</a> to join.",
        //            IsBodyHtml = true
        //        };
        //        db.Invites.Add(invite);
        //        db.SaveChanges();

        //        var svc = new PersonalEmail();
        //        await svc.SendAsync(email);

        //        return RedirectToAction("Details", "Households", new { id = id });
        //    }

        //    ViewBag.HouseholdId = id;
        //    return View(invite);
        //}




        //try
        //{
        //    var body = "<p>Email From: <b>{0}</b>({1})</p><p>Message:</p><p>{2}</p>";
        //    model. = "This is a message from your personal site. The name and the email of the contacting person is above";
        //    var email = new MailMessage(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["emailfrom"])
        //    {
        //        Subject = "Welcome to ericwatcom",
        //        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
        //        IsBodyHtml = true
        //    };

        //    var svc = new PersonalEmail();
        //    await svc.SendAsync(email);

        //    return RedirectToAction("EmailConfirmation");

        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    await Task.FromResult(0);
        //}
        //}
    }
}
