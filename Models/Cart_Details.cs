
namespace ShoppingCart_Application_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cart_Details
    {
        public int CD_ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
    
        public virtual Product_Table Product_Table { get; set; }
        public virtual RegisterUser RegisterUser { get; set; }
    }
}
