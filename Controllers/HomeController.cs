using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart_Application_MVC.Models;

namespace ShoppingCart_Application_MVC.Controllers
{
    // authorize is used to protect the pages from users without login they will not permission to access this
    /* [Authorize]*/
 /*   [AllowAnonymous]*/

    public class HomeController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();
        public ActionResult Index()
        {
            try
            {

              var Items = db.Products.ToList();
       
               /* TempData["ProductList"] = Items;*/

                if (Items.Count > 0)
                {
                    return View(Items);
                }
                else
                {
                    ViewBag.ProductsNotFound = "Products Not Found !...";
                }
            }
            catch (Exception ex) { 
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }

        public ActionResult Men()
        {
            try
            {
                ReturnProductList(1);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            /*try
            {
                var Items = db.Products.Where(p => p.CategoryID == 1).ToList();
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
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }*/
            return View();
        }

        public ActionResult Women()
        {
            try
            {
                ReturnProductList(2);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }
        public ActionResult Kids()
        {
            try
            {
                ReturnProductList(3);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }
        public ActionResult Home_Leaving()
        {
            try
            {
                ReturnProductList(4);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }
        public ActionResult Beuty()
        {
            try
            {
                ReturnProductList(5);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }
        public ActionResult Electronics()
        {
            try
            {
                ReturnProductList(6);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
            }
            return View();
        }

        // This method will return the all Products List

        public ActionResult ReturnProductList(int CategoryID)
        {
            try
            {
                var Items = db.Products.Where(p => p.CategoryID == CategoryID).ToList();
               /* TempData["ProductList"] = Items;*/

                if (Items.Count > 0)
                {
                    return View(Items);
                }
                else
                {
                    ViewBag.ProductsNotFound = "Products Not Available !...";
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