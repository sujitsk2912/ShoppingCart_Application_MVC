using ShoppingCart_Application_MVC.Models;
using ShoppingCart_Application_MVC.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly Shopping_CartEntities db = new Shopping_CartEntities(); // Assuming this is your DbContext

        [HttpGet]
        public ActionResult Product_List()
        {
            try
            {
                var productList = db.Products.ToList();
                var categories = db.Categories.ToList();
                var subCategories = db.SubCategories.ToList();

                if (productList != null && categories != null && subCategories != null)
                {
                    var adminCombineViewModel = new AdminCombineViewModel
                    {
                        Products = productList,
                        Categories = categories,
                        SubCategories = subCategories
                    };

                    return View(adminCombineViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Log the exception
            }

            return View(new AdminCombineViewModel()); // Return an empty view model if something goes wrong
        }

        public ActionResult Product_Add()
        {

            return View();
        }

        public ActionResult Category_List()
        {

            return View();
        }

        public ActionResult Order_List()
        {

            return View();
        }

        public ActionResult Order_Details()
        {

            return View();
        }
    }
}