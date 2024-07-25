using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    public class ProfileController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        // GET: Profile

        [HttpGet]
        public ActionResult Profile()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        if (userID > 0)
                        {
                            var UserDetails = db.RegisterUser.FirstOrDefault(x => x.UserID == userID);

                            if (UserDetails != null)
                            {
                                var UpdateProfile = new UpdateProfileViewModel()
                                {
                                    FirstName = UserDetails.FirstName,
                                    LastName = UserDetails.LastName,
                                    Email = UserDetails.Email,
                                    Phone = UserDetails.Phone,
                                };

                                return View(UpdateProfile);
                            }
                            else
                            {
                                ModelState.AddModelError("", "User details not found.");
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "You are not a registered user. Please log in first.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            return View(new UpdateProfileViewModel());
        }


        [HttpPost]
        public ActionResult Profile(UpdateProfileViewModel user)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        if (userID > 0)
                        {
                            var UserDetails = db.RegisterUser.Find(userID);

                            if (UserDetails != null)
                            {
                                // Check if the email is used by another user
                                var emailExists = db.RegisterUser.Any(x => x.Email == user.Email && x.UserID != userID);
                                if (emailExists)
                                {
                                    ViewBag.errorMessege = "This email is already in use by another user.";
                                }

                                // Check if the phone number is used by another user
                                var phoneExists = db.RegisterUser.Any(x => x.Phone == user.Phone && x.UserID != userID);
                                if (phoneExists)
                                {
                                    ViewBag.errorMessege = "This phone number is already in use by another user.";
                                }

                                if (!ModelState.IsValid)
                                {
                                    return View(user);
                                }

                                string updateQuery = "UPDATE RegisterUser SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone WHERE UserID = @UserID";
                                db.Database.ExecuteSqlCommand(updateQuery,
                                    new SqlParameter("@FirstName", user.FirstName),
                                    new SqlParameter("@LastName", user.LastName),
                                    new SqlParameter("@Email", user.Email),
                                    new SqlParameter("@Phone", user.Phone),
                                    new SqlParameter("@UserID", userID));

                                return RedirectToAction("Profile");
                            }
                            else
                            {
                                ModelState.AddModelError("", "User details not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            return View(user);
        }


    }
}