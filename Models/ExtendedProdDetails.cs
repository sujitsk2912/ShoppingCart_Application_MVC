using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC.Models
{
    public class ExtendedProdDetails
    {
        public List<usp_GetAllProdDetails_Result> AuthenticatedProducts { get; set; }
        public List<Product_Table> UnauthenticatedProducts { get; set; }
    }
}