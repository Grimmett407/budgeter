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

namespace budgeter.Controllers
{
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        public ActionResult Index()
        {
            return View(db.BudgetItems.ToList());
        }

        // GET: BudgetItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // GET: BudgetItems/Create
        public ActionResult Create(int householdId, int budgetId)
        {
            Budget budget = db.Budgets.Find(budgetId);
            List<BudgetItem> budgetItems = db.BudgetItems.Where(i => i.BudgetId == budgetId).ToList();
            List<Category> allCats = db.Categories.ToList();
            List<Category> usedCats = new List<Category>();
            List<Category> listCats = db.Categories.ToList();

            foreach (Category category in allCats)
            {
                foreach (BudgetItem budgetItem in budgetItems)
                {
                    if (category.Id == budgetItem.CategoryId)
                    {
                        listCats.Remove(category);
                    }
                }
            }

            ViewBag.HouseholdId = householdId;
            ViewBag.CategoryId = new SelectList(listCats.Where(c => c.Name != "Income"), "Id", "Name");
            return View();
        }


        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,BudgetId,Amount,Deleted")] BudgetItem item, int householdId, int budgetId)
        {
            if (ModelState.IsValid)
            {
                item.BudgetId = budgetId;
                var budget = db.Budgets.Find(item.BudgetId);
                var category = db.Categories.Find(item.CategoryId);
                db.BudgetItems.Add(item);
                db.SaveChanges();
                return RedirectToAction("Details", "Households", new { id = householdId });
            }

            ViewBag.BudgetId = budgetId;
            ViewBag.HouseholdId = householdId;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: BudgetItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount,BudgetId,CategoryId")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            db.BudgetItems.Remove(budgetItem);
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
    }
}
