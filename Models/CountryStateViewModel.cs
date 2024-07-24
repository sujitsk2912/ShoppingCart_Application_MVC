using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart_Application_MVC.Models
{
    public class CountryStateViewModel
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int PhoneCode { get; set; }

        public int StateID { get; set; }

        public string StateName { get; set; }

    }
}