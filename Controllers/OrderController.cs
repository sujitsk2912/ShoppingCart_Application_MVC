using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        public ActionResult Payment()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int userID = Convert.ToInt32(User.Identity.Name);

                    if (userID > 0)
                    {
                        var paymentAmount = db.PaymentAmounts.FirstOrDefault(u => u.UserID == userID);
                        var addressDetails = db.AddressDetails.FirstOrDefault(u => u.UserID == userID);

                        if (paymentAmount != null || addressDetails != null)
                        {
                            var profileCombineViewModel = new ProfileCombineViewModel
                            {
                                PaymentAmounts = paymentAmount,
                                UpdateAddress = addressDetails
                            };

                            db.SaveChanges();

                            TempData["isSuccess"] = true;

                            return View(profileCombineViewModel);
                        }
                        else
                        {
                            return RedirectToAction("AddToCart", "Cart");
                        }
                    }
                    else
                    {
                        return RedirectToAction("AddToCart", "Cart");
                    }
                }
                else
                {
                    return RedirectToAction("LoginUser", "Account");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AddToCart", "Cart");
        }

        public ActionResult Success()
        {
            return View();
        }

    }
}