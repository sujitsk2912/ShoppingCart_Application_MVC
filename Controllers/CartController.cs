﻿using ShoppingCart_Application_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects; // For ObjectParameter
using System.Data.Entity.Infrastructure; // For IObjectContextAdapter
using System.Threading.Tasks; // For Task
using System.Linq; // For ToList()


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
            return View(new List<ShoppingCart_Application_MVC.Models.usp_GetAllProdDetails_Result>());
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

                        if (cartItem != null)
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

                                cartItem.Quantity = Quantity;
                                cartItem.TotalPrice = cartItem.Quantity * product.ProductPrice;
                                db.SaveChanges();

                                var productList = db.usp_GetAllProdDetails(userID).ToList();
                                return View(productList);
                        }
                        else
                        {
                            var product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

                            Cart_Details cart = new Cart_Details()
                            {
                                UserID = userID,
                                ProductID = ProductID,
                                Quantity = Qty,
                                TotalPrice = product.ProductPrice * Qty,
                            };

                            db.Cart_Details.Add(cart);
                            db.SaveChanges();

                            var productList = db.usp_GetAllProdDetails(userID).ToList();
                            return View(productList);
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
            return View(new List<ShoppingCart_Application_MVC.Models.usp_GetAllProdDetails_Result>());
        }

        public ActionResult RemoveProduct(string PID, string UID)
        {
            try
            {
                int ProductID = Convert.ToInt32(PID);
                int UserID = Convert.ToInt32(UID);

                if (User.Identity.IsAuthenticated)
                {
                    if (ProductID != 0 && UserID != 0)
                    {
                        var removeProduct = db.Cart_Details.FirstOrDefault(p => p.ProductID == ProductID && p.UserID == UserID);

                        if (removeProduct != null)
                        {
                            db.Cart_Details.Remove(removeProduct);

                            db.SaveChanges();

                            var productList = db.usp_GetAllProdDetails(UserID).ToList();
                            return View(productList);
                        }
                    }
                }
                else
                {

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

        [HttpPost]
        public ActionResult OrderInfo(AddressDetails details)
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
                            if (details != null)
                            {
                                var AddressDetails = new AddressDetails()
                                {
                                    FirstName = details.FirstName,
                                    LastName = details.LastName,
                                    Phone = details.Phone,
                                    Email = details.Email,
                                    Address = details.Address,
                                    Landmark = details.Landmark,
                                    HouseNo = details.HouseNo,
                                    Country = details.Country,
                                    State = details.State,
                                    City = details.City,
                                    Pincode = details.Pincode,
                                    UserID = userID,
                                    isSaved = details.isSaved,
                                    addressType = details.addressType
                                };

                                db.AddressDetails.Add(AddressDetails);
                                db.SaveChanges();

                                ClearResult();

                                return View();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }
                        return View();
                    }
                }
                else
                {
                    Response.Write("You are not registerd user Login first");
                    ClearResult();
                    return View();
                }
            }
            return View(details);
        }

        public ActionResult Payment()
        {
            return View();
        }

        public void ClearResult()
        {
            
            if (ModelState.IsValid)
            {
                /*var AddressDetails = new AddressDetails()
                {
                    FirstName = "",
                    LastName = "",
                    Phone = ,
                    Email = details.Email,
                    Address = details.Address,
                    Landmark = details.Landmark,
                    HouseNo = details.HouseNo,
                    Country = details.Country,
                    State = details.State,
                    City = details.City,
                    Pincode = details.Pincode,
                    UserID = userID,
                    isSaved = details.isSaved,
                    addressType = details.addressType
                };*/
/*
                db.AddressDetails.Add(AddressDetails);
                db.SaveChanges();*/
            }
            
        }
    }
}
