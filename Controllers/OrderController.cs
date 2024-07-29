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
                    int userID;
                    if (int.TryParse(User.Identity.Name, out userID) && userID > 0)
                    {
                        var paymentAmount = db.PaymentAmounts.FirstOrDefault(u => u.UserID == userID);

                        if (paymentAmount != null)
                        {
                            var paymentList = new ProfileCombineViewModel
                            {
                                PaymentAmounts = paymentAmount
                            };

                            return View(paymentList);
                        }
                       /* else
                        {
                            return RedirectToAction("AddToCart", "Cart");
                        }*/
                    }
                    else
                    {
                      /*  return RedirectToAction("AddToCart", "Cart");*/
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
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}