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
        public ActionResult Profile(string addressType)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        // Default addressType to "HomeAddress" if not provided
                        if (string.IsNullOrEmpty(addressType))
                        {
                            addressType = "HomeAddress";
                        }

                        var viewModel = new ProfileCombineViewModel();

                        if (userID > 0)
                        {
                            // Address details not found, fetch user profile
                            var userDetails = db.RegisterUser.FirstOrDefault(x => x.UserID == userID);

                            if (userDetails != null)
                            {

                                viewModel.UpdateProfile = new UpdateProfileViewModel
                                {
                                    FirstName = userDetails.FirstName,
                                    LastName = userDetails.LastName,
                                    Email = userDetails.Email,
                                    Phone = userDetails.Phone,
                                };
                            }
                            else
                            {
                                ModelState.AddModelError("", "User details not found.");
                            }
                        }

                        // Fetch address details based on addressType
                        var getAddressDetails = db.AddressDetails
                            .FirstOrDefault(p => p.addressType == addressType && p.UserID == userID);

                        if (getAddressDetails != null)
                        {
                            viewModel.UpdateAddress = new AddressDetails1
                            {
                                FirstName = getAddressDetails.FirstName,
                                LastName = getAddressDetails.LastName,
                                Phone = getAddressDetails.Phone,
                                Email = getAddressDetails.Email,
                                Address = getAddressDetails.Address,
                                Landmark = getAddressDetails.Landmark,
                                HouseNo = getAddressDetails.HouseNo,
                                Country = getAddressDetails.Country,
                                State = getAddressDetails.State,
                                City = getAddressDetails.City,
                                Pincode = getAddressDetails.Pincode,
                                UserID = userID,
                                isSaved = getAddressDetails.isSaved,
                                addressType = getAddressDetails.addressType
                            };
                        }

                        return View(viewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are not a registered user. Please log in first.");
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

            // Return default view or error view if no conditions are met
            return View(new ProfileCombineViewModel()); // Replace with your actual view name
        }



        [HttpPost]
        public ActionResult Profile(ProfileCombineViewModel user)
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
                                var emailExists = db.RegisterUser.Any(x => x.Email == user.UpdateProfile.Email && x.UserID != userID);
                                if (emailExists)
                                {
                                    ViewBag.errorMessege = "This email is already in use by another user.";
                                }

                                // Check if the phone number is used by another user
                                var phoneExists = db.RegisterUser.Any(x => x.Phone == user.UpdateProfile.Phone && x.UserID != userID);
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
                                    new SqlParameter("@FirstName", user.UpdateProfile.FirstName),
                                    new SqlParameter("@LastName", user.UpdateProfile.LastName),
                                    new SqlParameter("@Email", user.UpdateProfile.Email),
                                    new SqlParameter("@Phone", user.UpdateProfile.Phone),
                                    new SqlParameter("@UserID", userID));

                                /*return RedirectToAction("Profile");*/
                            }
                            else
                            {
                                ModelState.AddModelError("", "User details not found.");
                            }
                        }

                        // Check if the address already exists
                        var existingAddress = db.AddressDetails.FirstOrDefault(a => a.addressType == user.UpdateAddress.addressType && a.UserID == userID);

                        if (existingAddress != null)
                        {
                            // Update existing address
                            existingAddress.FirstName = user.UpdateAddress.FirstName;
                            existingAddress.LastName = user.UpdateAddress.LastName;
                            existingAddress.Phone = user.UpdateAddress.Phone;
                            existingAddress.Email = user.UpdateAddress.Email;
                            existingAddress.Address = user.UpdateAddress.Address;
                            existingAddress.Landmark = user.UpdateAddress.Landmark;
                            existingAddress.HouseNo = user.UpdateAddress.HouseNo;
                            existingAddress.Country = user.UpdateAddress.Country;
                            existingAddress.State = user.UpdateAddress.State;
                            existingAddress.City = user.UpdateAddress.City;
                            existingAddress.Pincode = user.UpdateAddress.Pincode;
                            existingAddress.isSaved = user.UpdateAddress.isSaved;
                            existingAddress.addressType = user.UpdateAddress.addressType;

                            db.Entry(existingAddress).State = System.Data.Entity.EntityState.Modified;

                            ViewBag.SuccessAddress = "Address Updated Successfully...";

                            db.SaveChanges();

                        }

                        return View(new ProfileCombineViewModel());
                    }
                }
                else
                {
                    Response.Write("Please login first.");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            return View(user);
        }



        /*[HttpPost]
        public ActionResult Address(ProfileCombineViewModel address)
        {
            return View();
        }*/

        public ActionResult ChangePassword()
        {
            return View();
        }

    }
}