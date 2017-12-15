using budgeter.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace budgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //var user = db.Users.Find(User.Identity.GetUserId());
            //var role = db.Users.SingleOrDefault(u => u.Id == );
            //var usersInRole = db.Users.Where(u => u.Roles.Any(r => (r.RoleId == role.Id)));
            //ViewBag.AssignedId = new SelectList(usersInRole, "Id", "FirstName");
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Households.ToList());
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