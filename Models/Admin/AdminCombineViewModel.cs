using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC.Models.Admin
{
    public class AdminCombineViewModel
    {
        public List<Categories> Categories { get; set; }
        public List<SubCategories> SubCategories { get; set; }
        public List<Product_Table> Products { get; set; }
    }
}