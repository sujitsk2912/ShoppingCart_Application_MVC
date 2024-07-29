using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Filters
{
    public class ValidatePaymentDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var paymentAmount = session["PaymentAmount"] as PaymentAmountViewModel;

            // Check if the error message should be set
            var isPaymentPage = filterContext.ActionDescriptor.ActionName == "Payment" && filterContext.HttpContext.Request.HttpMethod == "GET";

            if (paymentAmount == null && isPaymentPage)
            {
                filterContext.Controller.ViewBag.ErrorMessage = "Please add items to your cart before proceeding to payment.";
                filterContext.Controller.ViewBag.IsPaymentDataAvailable = false;
            }
            else
            {
                filterContext.Controller.ViewBag.IsPaymentDataAvailable = true;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}