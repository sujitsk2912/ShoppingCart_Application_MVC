using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
                            viewModel.UpdateAddress = new AddressDetails
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
                        return RedirectToAction("LoginUser", "Account" );
                    }
                }
                else
                {
                    return RedirectToAction("LoginUser", "Account");


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
                            if (user.UpdateProfile != null)
                            {
                                var UserDetails = db.RegisterUser.Find(userID);

                                if (UserDetails != null)
                                {
                                    // Check if the email is used by another user
                                    var emailExists = db.RegisterUser.Any(x => x.Email == user.UpdateProfile.Email && x.UserID != userID);
                                    if (emailExists)
                                    {
                                        TempData["ErrorMessage"] = "This email is already in use by another user.";
                                    }

                                    // Check if the phone number is used by another user
                                    var phoneExists = db.RegisterUser.Any(x => x.Phone == user.UpdateProfile.Phone && x.UserID != userID);
                                    if (phoneExists)
                                    {
                                        TempData["ErrorMessage"] = "This phone number is already in use by another user.";
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

                                    TempData["SuccessMessage"] = "Profile Updated Successfully...";
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "User details not found.";
                                }
                            }

                            /*// Handle address update logic
                            var existingAddress = db.AddressDetails.FirstOrDefault(a => a.addressType == user.UpdateAddress.addressType && a.UserID == userID);

                            if (existingAddress != null)
                            {
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

                                TempData["SuccessAddress"] = "Address Updated Successfully...";

                                db.SaveChanges();
                            }*/

                            return RedirectToAction("Profile");
                        }
                    }
                    else
                    {
                        Response.Write("Please login first.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            return RedirectToAction("Profile");
        }




        [HttpPost]
        public ActionResult Address(ProfileCombineViewModel addressDetails)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        try
                        {
                            // Check if the address already exists
                            var existingAddress = db.AddressDetails.FirstOrDefault(a => a.addressType == addressDetails.UpdateAddress.addressType && a.UserID == userID);

                            if (existingAddress != null)
                            {
                                // Update existing address
                                existingAddress.FirstName = addressDetails.UpdateAddress.FirstName;
                                existingAddress.LastName = addressDetails.UpdateAddress.LastName;
                                existingAddress.Phone = addressDetails.UpdateAddress.Phone;
                                existingAddress.Email = addressDetails.UpdateAddress.Email;
                                existingAddress.Address = addressDetails.UpdateAddress.Address;
                                existingAddress.Landmark = addressDetails.UpdateAddress.Landmark;
                                existingAddress.HouseNo = addressDetails.UpdateAddress.HouseNo;
                                existingAddress.Country = addressDetails.UpdateAddress.Country;
                                existingAddress.State = addressDetails.UpdateAddress.State;
                                existingAddress.City = addressDetails.UpdateAddress.City;
                                existingAddress.Pincode = addressDetails.UpdateAddress.Pincode;
                                existingAddress.isSaved = addressDetails.UpdateAddress.isSaved;
                                existingAddress.addressType = addressDetails.UpdateAddress.addressType;

                                db.Entry(existingAddress).State = System.Data.Entity.EntityState.Modified;

                                TempData["SuccessMessage"] = "Address Updated Successfully...";

                                db.SaveChanges();

                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }

                        return RedirectToAction("Profile");
                    }
                }
                else
                {
                    Response.Write("Please login first.");
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public ActionResult ChangePassword(ProfileCombineViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        try
                        {
                            string passwordHashOld = HashPassword(model.ChangePassword.OldPassword);
                            string passwordHashNew = HashPassword(model.ChangePassword.NewPassword);

                            var checkPassword = db.RegisterUser.FirstOrDefault(c => c.Password == passwordHashOld && c.UserID == userID);

                            if (checkPassword != null)
                            {
                                if (model.ChangePassword.NewPassword == model.ChangePassword.ConfirmPassword)
                                {
                                    checkPassword.Password = passwordHashNew;
                                    checkPassword.ConfirmPassword = passwordHashNew;

                                    db.Entry(checkPassword).State = System.Data.Entity.EntityState.Modified;

                                    TempData["SuccessMessage"] = "Password Changed Successfully...";
                                    db.SaveChanges();

                                    // Redirect to a view that shows the message before logging out
                                    return RedirectToAction("Profile");
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "Password Not Matched.";
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Old password is incorrect.";
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                        }
                    }
                }
            }

            return RedirectToAction("Profile");
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}