using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HospitalManagement.Models;
 
namespace HospitalManagement.Controllers
{
    public class UserController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: User
        public ActionResult Index()
        {
            var users = db.User.ToList();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.User
                             .FirstOrDefault(u => u.UserID == id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // GET: User/Create (Admin creates user)
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,Email,UserName,PasswordHash,IsActive,Address")] User user, HttpPostedFileBase ImageFile)
        {
            if (ImageFile == null || ImageFile.ContentLength == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image to upload.");
            }
            if (ModelState.IsValid)
            {
                string fileName = System.IO.Path.GetFileName(ImageFile.FileName);
                string uploadPath = Server.MapPath("~/Content/Images/" + fileName);

                // Save file
                ImageFile.SaveAs(uploadPath);

                // Save relative path to DB
                user.ProfileImagePath = "/Content/Images/" + fileName;

                user.PasswordHash = HashPassword(user.PasswordHash);
                user.CreatedAt = DateTime.Now;
                db.User.Add(user);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.User.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                var userInDb = db.User.Find(model.UserID);
                if (userInDb == null)
                    return HttpNotFound();

                userInDb.FullName = model.FullName;
                userInDb.Email = model.Email;
                userInDb.UserName = model.UserName;
                userInDb.IsActive = model.IsActive;
                userInDb.Address = model.Address;

                // --- NEW: Handle Profile Image Upload ---
                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ProfileImage.FileName);
                    string uploadPath = Server.MapPath("~/Content/Images/");
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    string fullPath = System.IO.Path.Combine(uploadPath, fileName);
                    ProfileImage.SaveAs(fullPath);

                    // Save relative path to DB
                    userInDb.ProfileImagePath = "/Content/Images/" + fileName;
                }

                db.Entry(userInDb).State = EntityState.Modified;
                db.SaveChanges();

                // Optional: update session to immediately reflect the new image
                Session["ProfileImage"] = userInDb.ProfileImagePath;

                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = db.User.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User user = db.User.Find(id);
                if (user != null)
                {
                    db.User.Remove(user);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                // This exception occurs when foreign key constraint prevents deletion
                TempData["Error"] = "Cannot delete this user because they are linked to one or more articles.";
                return RedirectToAction("Index");
            }
        }

        // GET: Signup
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "FullName,Email,UserName,PasswordHash,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.User.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(user);
                }

                if (db.User.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("", "Email already exists.");
                    return View(user);
                }

                user.PasswordHash = HashPassword(user.PasswordHash);
                user.IsActive = true;
                user.CreatedAt = DateTime.Now;

                db.User.Add(user);
                db.SaveChanges();

                // Optionally log in user immediately
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["FullName"] = user.FullName;
                Session["Address"] = user.Address;
                Session["ProfileImage"] = user.ProfileImagePath;

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            string hashed = HashPassword(password);
            var user = db.User.FirstOrDefault(u => u.UserName == username && u.PasswordHash == hashed && u.IsActive);

            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["FullName"] = user.FullName;
                Session["Address"] = user.Address;
                Session["ProfileImage"] = user.ProfileImagePath;

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Helper method: SHA256 hashing
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
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
