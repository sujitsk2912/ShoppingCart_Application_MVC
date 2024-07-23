using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC.Models
{
    public class CartItemViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}