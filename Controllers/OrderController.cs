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

        [HttpGet]
        public ActionResult Payment(string paymentId)
        {
           /* var storedPaymentId = Session["UniquePaymentId"] as string;
            if (string.IsNullOrEmpty(paymentId) || paymentId != storedPaymentId)
            {
                return RedirectToAction("AddToCart", "Cart");
            }

            // Retrieve necessary data for the PaymentAmountViewModel
            PaymentAmountViewModel paymentAmount = GetPaymentAmountViewModelFromSessionOrDatabase();

            var model = new ProfileCombineViewModel
            {
                Order = paymentAmount ?? new PaymentAmountViewModel()
            };

            return View(model);*/
           return View();
        }


        [HttpPost]
        public ActionResult Payment(PaymentAmountViewModel model, string paymentId)
        {
          /*  try
            {
                var storedPaymentId = Session["UniquePaymentId"] as string;
                if (paymentId != storedPaymentId)
                {
                    return RedirectToAction("AddToCart", "Cart");
                }

                if (model != null)
                {
                    var amountDetails = new ProfileCombineViewModel
                    {
                        Order = model
                    };

                    // Save model to session or database if needed
                    SavePaymentAmountViewModelToSessionOrDatabase(model);

                    // Return a view or redirect based on your logic
                    return View(amountDetails);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it accordingly
                Response.Write(ex.Message);
            }*/

            return View(new ProfileCombineViewModel { Order = model ?? new PaymentAmountViewModel() });
        }


        [HttpPost]
        public ActionResult StorePaymentId(string paymentId)
        {

            /*// Check if the paymentId is null or empty
            if (string.IsNullOrEmpty(paymentId))
            {
                // Generate a new unique payment ID if needed (this is just an example)
                paymentId = Guid.NewGuid().ToString();

                // Store the new payment ID in the session
                Session["UniquePaymentId"] = paymentId;
            }*/

            // If paymentId is not null or empty, return the existing payment ID from the session
           if (Session["UniquePaymentId"] != null)
            {
                paymentId = Session["UniquePaymentId"].ToString();
            }

            // Return the payment ID in JSON format
            return Json(new { paymentId = paymentId }, JsonRequestBehavior.AllowGet);
        }

        private PaymentAmountViewModel GetPaymentAmountViewModelFromSessionOrDatabase()
        {
            PaymentAmountViewModel paymentAmount = Session["PaymentAmount"] as PaymentAmountViewModel;
            if (paymentAmount == null)
            {
                // Retrieve from database if needed
                // paymentAmount = db.PaymentAmountViewModel.FirstOrDefault();
            }
            return paymentAmount;
        }

        private void SavePaymentAmountViewModelToSessionOrDatabase(PaymentAmountViewModel model)
        {
            Session["PaymentAmount"] = model;
        }

    }
}