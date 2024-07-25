using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    public class ProfileController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        // GET: Profile
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
                                return View(UserDetails);
                            }
                        }
                    }
                }
                else
                {
                    Response.Write("You are not a registered user. Please login first.");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Profile(RegisterUser user)
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
                                var UpdateUser = new RegisterUser()
                                {
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Email = user.Email,
                                    Phone = user.Phone,
                                };

                                db.Entry(UpdateUser).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();

                                return View(UserDetails);
                            }
                            else
                            {
                                return View();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return View();
        }
    }
}