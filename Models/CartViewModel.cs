using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC.Models
{
    public class CartViewModel
    {
        public List<usp_GetAllProdDetails_Result> Authenticated { get; set; }
        public List<CartItemViewModel> Unauthenticated { get; set; }

        public PaymentAmounts PaymentAmounts { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}