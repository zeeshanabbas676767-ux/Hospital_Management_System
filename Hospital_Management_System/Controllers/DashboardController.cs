using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Home
        public ActionResult Home()  
        {
            // ✅ Check if user is logged in
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Pass user info to the View
            ViewBag.UserName = Session["UserName"];
            ViewBag.Email = Session["Email"];
            return View();
        }
        public ActionResult Logout()
        {
            // ✅ Clear session and redirect to Login
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}