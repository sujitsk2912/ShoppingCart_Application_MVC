//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingCart_Application_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product_Table
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product_Table()
        {
            this.Cart_Details = new HashSet<Cart_Details>();
            this.Wishlist = new HashSet<Wishlist>();
        }
    
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public byte[] ProductImage { get; set; }
        public Nullable<decimal> ProductPrice { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public Nullable<int> RemainingQuantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart_Details> Cart_Details { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual SubCategories SubCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlist { get; set; }
    }
}
