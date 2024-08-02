using ShoppingCart_Application_MVC.Models;
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
            var model = new CartViewModel();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int userID = int.Parse(User.Identity.Name);

                    model.IsAuthenticated = true;

                    if (userID != 0)
                    {
                        var cartProductCount = db.Cart_Details.Where(u => u.UserID == userID).ToList();

                        Session["ItemsCount"] = cartProductCount.Count();

                        var productList = db.usp_GetAllProdDetails(userID).ToList();

                        if (productList.Count > 0)
                        {
                            model.Authenticated = productList;
                        }
                        else
                        {
                            ViewBag.ProductsNotFound = "Your cart is Empty!";
                        }
                    }
                }
                else
                {
                    model.IsAuthenticated = false;

                    var productList = Session["ProductList"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();

                    if (productList.Count > 0)
                    {
                        model.Unauthenticated = productList;

                        Session["ItemsCount"] = productList.Count();

                    }
                    else
                    {
                        ViewBag.ProductsNotFound = "Your cart is Empty!";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToCart(string ID, int Quantity)
        {
            int Qty = Convert.ToInt32(Quantity);
            int ProductID = Convert.ToInt32(ID);

            try
            {
                if (User.Identity.IsAuthenticated)
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
                            return RedirectToAction("AddToCart");
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

                            return RedirectToAction("AddToCart");
                        }
                    }
                }
                else
                {
                    // Logic for unauthorized users
                    var productList = Session["ProductList"] as List<CartItemViewModel>;

                    if (productList == null)
                    {
                        productList = new List<CartItemViewModel>();
                    }

                    var product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

                    if (product != null)
                    {
                        var existingProduct = productList.FirstOrDefault(p => p.ProductID == ProductID);

                        if (existingProduct != null)
                        {
                           /* existingProduct.Quantity += Qty;
                            existingProduct.TotalPrice = existingProduct.Quantity * product.ProductPrice;
                        */}
                        else
                        {
                            var newProduct = new CartItemViewModel
                            {
                                ProductID = ProductID,
                                ProductName = product.ProductName,
                                Description = product.Description,
                                ProductPrice = Convert.ToDecimal(product.ProductPrice),
                                Quantity = Qty,
                                TotalPrice = Convert.ToDecimal(product.ProductPrice * Qty)
                            };

                            productList.Add(newProduct);
                        }

                        Session["ProductList"] = productList;
                    }

                    return RedirectToAction("AddToCart");


                    // Update the model for the view
                    /*var model = new CartViewModel
                    {
                        IsAuthenticated = false,
                        Unauthenticated = productList
                    };

                    return View(model);*/ // Return the updated view directly
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            return RedirectToAction("AddToCart");
        }

        [HttpPost]
        public JsonResult UpdatePrices(float price, int items, int discountPercentage, float discountAmount, float deliveryCharges, float totalAmount)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int userID = int.Parse(User.Identity.Name);

                    if (userID > 0)
                    {
                        var paymentDetail = db.PaymentAmounts.FirstOrDefault(p => p.UserID == userID);

                        if (paymentDetail != null)
                        {
                            // Update payment details
                            paymentDetail.Price = Convert.ToDecimal(price);
                            paymentDetail.Items = items;
                            paymentDetail.DiscountPercentage = discountPercentage;
                            paymentDetail.DiscountAmount = Convert.ToDecimal(discountAmount);
                            paymentDetail.DeliveryCharges = Convert.ToDecimal(deliveryCharges);
                            paymentDetail.TotalAmount = Convert.ToDecimal(totalAmount);
                        }
                        else
                        {
                            // Add new payment details
                            PaymentAmounts newPaymentDetail = new PaymentAmounts()
                            {
                                UserID = userID,
                                Price = Convert.ToDecimal(price),
                                Items = items,
                                DiscountPercentage = discountPercentage,
                                DiscountAmount = Convert.ToDecimal(discountAmount),
                                DeliveryCharges = Convert.ToDecimal(deliveryCharges),
                                TotalAmount = Convert.ToDecimal(totalAmount)
                            };
                            db.PaymentAmounts.Add(newPaymentDetail);
                        }

                        ViewBag.Items = items;

                        db.SaveChanges();

                        return Json(new { success = true, message = "Update successful" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                // Logging code here
                return Json(new { success = false, message = "An error occurred" });
            }

            return Json(new { success = false, message = "User not authenticated or invalid userID" });
        }

        public ActionResult RemoveProductAuthenticated(string PID, string UID)
        {
            try
            {
                int ProductID = Convert.ToInt32(PID);
                int UserID = Convert.ToInt32(UID);

                if (User.Identity.IsAuthenticated)
                {
                    if (ProductID != 0 && UserID != 0)
                    {
                        var productList = db.Cart_Details.Where(u => u.UserID == UserID).ToList();
                        var removeProduct = db.Cart_Details.FirstOrDefault(p => p.ProductID == ProductID && p.UserID == UserID);
                        var removeProductPayment = db.PaymentAmounts.FirstOrDefault(p => p.UserID == UserID);
                        
                        if (removeProduct != null)
                        {
                            db.Cart_Details.Remove(removeProduct);

                            db.SaveChanges();


                            var cartProductCount = db.Cart_Details.Where(u => u.UserID == UserID).ToList();

                            Session["ItemsCount"] = cartProductCount.Count();

                            /*                            TempData["SuccessMessage"] = "Product Removed Successfully...";
                            */

                            /*  var productList = db.usp_GetAllProdDetails(UserID).ToList();

                              return View(productList);*/
                        }
                        if (productList.Count() == 1)
                        {
                            db.PaymentAmounts.Remove(removeProductPayment);
                            db.SaveChanges();
                        }

                        return RedirectToAction("AddToCart");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return RedirectToAction("AddToCart");
        }

        public ActionResult RemoveProductUnauthenticated(string PID)
        {
            try
            {
                int ProductID = Convert.ToInt32(PID);

                if (!User.Identity.IsAuthenticated)
                {
                    var productList = Session["ProductList"] as List<CartItemViewModel>;

                    if (productList != null)
                    {
                        var productToRemove = productList.FirstOrDefault(p => p.ProductID == ProductID);
                        if (productToRemove != null)
                        {
                            productList.Remove(productToRemove);
                            Session["ProductList"] = productList;

                            Session["ItemsCount"] = productList.Count();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return RedirectToAction("AddToCart");
        }

        [HttpGet]
        public ActionResult OrderInfo(string addressType)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        int userID = int.Parse(User.Identity.Name);

                        try
                        {
                            if (string.IsNullOrEmpty(addressType))
                            {
                                addressType = "HomeAddress";
                            }

                            var getAddressDetails = db.AddressDetails.FirstOrDefault(p => p.addressType == addressType && p.UserID == userID);

                            if (getAddressDetails != null)
                            {
                                var details = new AddressDetails()
                                {
                                    FirstName = getAddressDetails.FirstName,
                                    LastName = getAddressDetails.LastName,
                                    Phone = getAddressDetails.Phone,
                                    Email = getAddressDetails.Email,
                                    Address = getAddressDetails.Address,
                                    Landmark = getAddressDetails.Landmark,
                                    HouseNo = getAddressDetails.HouseNo,
                                    Country = getAddressDetails.Country,
                                    State = getAddressDetails.State,
                                    City = getAddressDetails.City,
                                    Pincode = getAddressDetails.Pincode,
                                    UserID = userID,
                                    isSaved = getAddressDetails.isSaved,
                                    addressType = getAddressDetails.addressType
                                };

                                return View(details);
                            }

                            return View(new AddressDetails());
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }
                    }
                }
                else
                {
                    return RedirectToAction("LoginUser", "Account");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            return View(new AddressDetails());
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
                            // Check if the address already exists
                            var existingAddress = db.AddressDetails.FirstOrDefault(a => a.addressType == details.addressType && a.UserID == userID);

                            if (existingAddress != null)
                            {
                                // Update existing address
                                existingAddress.FirstName = details.FirstName;
                                existingAddress.LastName = details.LastName;
                                existingAddress.Phone = details.Phone;
                                existingAddress.Email = details.Email;
                                existingAddress.Address = details.Address;
                                existingAddress.Landmark = details.Landmark;
                                existingAddress.HouseNo = details.HouseNo;
                                existingAddress.Country = details.Country;
                                existingAddress.State = details.State;
                                existingAddress.City = details.City;
                                existingAddress.Pincode = details.Pincode;
                                existingAddress.isSaved = details.isSaved;
                                existingAddress.addressType = details.addressType;

                                db.Entry(existingAddress).State = System.Data.Entity.EntityState.Modified;

                                ViewBag.SuccessAddress = "Address Updated Successfully...";

                                db.SaveChanges();

                            }
                            else
                            {
                                // Add new address
                                var addressDetails = new AddressDetails()
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

                                ViewBag.SuccessAddress = "Address Successfuly Added...";

                                db.AddressDetails.Add(addressDetails);

                                db.SaveChanges();

                                ModelState.Clear();

                            }

                            return View(new AddressDetails()); 
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }
                        return View(new AddressDetails()); 
                    }
                }
                else
                {
                    return RedirectToAction("LoginUser", "Account");

                }
            }
            return View(details);
        }

       
    }
}
