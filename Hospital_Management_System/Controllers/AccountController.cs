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
    public class AccountController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User user, string passwordConfirm) 
        {
            // Set required non-nullable fields BEFORE validation
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;

            // Check for duplicate email
            if (db.User.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
            }

            // Check password match
            if (user.PasswordHash != passwordConfirm)
            {
                ModelState.AddModelError("PasswordHash", "Passwords do not match.");
            }

            // 🔍 Debug any validation issue
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Key: {error.Key} | Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}"
                    );
                }
                return View(user); // re-render the same form
            }

            // ✅ Everything valid, hash and save
            user.PasswordHash = HashPassword(user.PasswordHash);

            db.User.Add(user);
            db.SaveChanges();

            // ✅ Auto login new user
            Session["UserID"] = user.UserID;
            Session["UserName"] = user.UserName;
            Session["Email"] = user.Email;
            Session["FullName"] = user.FullName;
            Session["Address"] = user.Address;

            TempData["Success"] = "Account created successfully! Welcome, " + user.UserName;
            return RedirectToAction("Login");
        }


        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string password, User model)
        {
            // Find user by username only first
            var user = db.User.FirstOrDefault(u => u.UserName == model.UserName);

            if (user != null)
            {
                // Hash the entered password and compare with stored hash
                string hashed = HashPassword(password);

                if (user.PasswordHash == hashed)
                {
                    if (user.IsActive)
                    {
                        // Save user info to session
                        Session["UserID"] = user.UserID;
                        Session["UserName"] = user.UserName;
                        Session["Email"] = user.Email;
                        Session["FullName"] = user.FullName;
                        Session["Address"] = user.Address;
                        Session["ProfileImage"] = string.IsNullOrEmpty(user.ProfileImagePath)
                            ? Url.Content("/Content/Images/default-avatar.png")
                            : Url.Content(user.ProfileImagePath);

                        TempData["Welcome"] = "Welcome, " + user.FullName + "!";
                        return RedirectToAction("Home", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Error = "Your account is inactive. Please contact support.";
                        return View(model);
                    }
                }
            }

            // If user not found or password incorrect
            ViewBag.Error = "Invalid username or password.";
            return View(model);
        }



        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Password Hashing
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        [HttpPost]
        public JsonResult UploadProfileImage(HttpPostedFileBase file, int userId)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("/Content/Images/"), fileName);
                file.SaveAs(path);

                var user = db.User.Find(userId);
                if (user != null)
                {
                    user.ProfileImagePath = "/Content/Images/" + fileName;
                    db.SaveChanges();

                    Session["ProfileImage"] = user.ProfileImagePath;

                    return Json(new { success = true, imageUrl = user.ProfileImagePath });
                }
            }

            return Json(new { success = false });
        }
        // GET: Account/Edit/5
        public ActionResult Setting(int? id)
        {

            User user = db.User.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(User model, HttpPostedFileBase ProfileImage)
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
                    string uploadPath = Server.MapPath("/Content/Images/");
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
    }
}
