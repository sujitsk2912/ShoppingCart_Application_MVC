using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart_Application_MVC.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        Shopping_CartEntities db = new Shopping_CartEntities();

        // GET: Cart

        public ActionResult AddToCart()
        {
            try
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    int userID = int.Parse(User.Identity.Name);

                    if (userID != 0)
                    {
                        var productList = db.usp_GetAllProdDetails(userID).ToList();

                        if (productList.Count > 0)
                        {
                            return View(productList);
                        }
                        else
                        {
                            ViewBag.ProductsNotFound = "Your cart is Empty !";
                        }
                    }
                }
                else
                {
                    // Fetch the product list from the session
                    var productList = Session["ProductList"] as List<Product_Table> ?? new List<Product_Table>();

                    if (productList != null && productList.Count > 0)
                    {
                        return View(productList);
                    }
                    else
                    {
                        ViewBag.ProductsNotFound = "Your cart is Empty !";
                    }
                }
            }
            catch (Exception ex) 
            {
                Response.Write(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(string ID, int Quantity)
        {
            int Qty = Convert.ToInt32(Quantity);
            int ProductID = Convert.ToInt32(ID);

            try
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    int userID = int.Parse(User.Identity.Name);

                    if (userID > 0)
                    {
                        var cartItem = db.Cart_Details.FirstOrDefault(p => p.UserID == userID && p.ProductID == ProductID);

                      /*  var cartItem1 = db.usp_GetAllProdDetails(userID).ToList();*/

                        if (cartItem != null)
                        {
                            if (cartItem.Quantity <= 8)
                            {
                                cartItem.Quantity += Quantity;
                                db.SaveChanges();

                                return View(cartItem);
                            }
                            else
                            {
                                ViewBag.cartIsFull = "Cannot add more than eight product's !";
                            }
                        }
                        else
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

                            Cart_Details cart = new Cart_Details()
                            {
                                UserID = userID,
                                ProductID = ProductID,
                                Quantity = Qty,
                                TotalPrice = product.ProductPrice * Qty
                            };

                            db.Cart_Details.Add(cart);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        var product = db.Products.SingleOrDefault(p => p.ProductID == ProductID);
                        
                        if (product != null)
                        {
                            var productList = Session["ProductList"] as List<Product_Table> ?? new List<Product_Table>();

                            productList.Add(product);

                            Session["ProductList"] = productList;

                            return View(productList);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Response.Write(ex.ToString());
            }
            return View();
        }

        public ActionResult OrderInfo()
        {

            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }
    }
}
