using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    public class ProductDetailsController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        // GET: Product
        [HttpGet]
        public ActionResult Product(string ID)
        {

            int ProductID = Convert.ToInt32(ID);

            try
            {
                var selectedProduct = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

                if (selectedProduct != null)
                {
                    var subCategoryProducts = db.Products
                                                .Where(p => p.SubCategoryID == selectedProduct.SubCategoryID && p.ProductID != ProductID)
                                                .ToList();

                    TempData["SubProductList"] = subCategoryProducts;

                    // Pass the selected product to the view
                    return View(new List<ShoppingCart_Application_MVC.Models.Product_Table> { selectedProduct });
                }
                else
                {
                    ViewBag.ProductsNotFound = "Product Not Found!...";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.StatusCode = 500;
                return View();
            }

            return View();
        }
    }
}