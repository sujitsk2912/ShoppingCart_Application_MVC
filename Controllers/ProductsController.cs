using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    public class ProductsController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        // GET: Products
        public ActionResult SearchProducts(string Query)
        {
            try
            {
                if (Query != "")
                {
                    var Items = db.Products.Where(p => p.ProductName.Contains(Query)).ToList();

                    TempData["ProductList"] = Items;

                    if (Items.Count > 0)
                    {
                        return View(Items);
                    }
                    else
                    {
                        ViewBag.ProductsNotFound = "Products Not Found !...";
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }
    }
}