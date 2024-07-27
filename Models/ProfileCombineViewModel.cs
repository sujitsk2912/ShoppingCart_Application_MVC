using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart_Application_MVC.Models
{
    public class UpdateProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class AddressDetails1
    {
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string HouseNo { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please select State")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pincode is Required")]
        public string Pincode { get; set; }

        public int UserID { get; set; }
        public bool isSaved { get; set; }
        public string addressType { get; set; }

        public virtual RegisterUser RegisterUser { get; set; }
    }

    public class OrdersViewModel
    {
    }

    public class ChangePasswordViewModel1
    {
        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileCombineViewModel
    {
        public UpdateProfileViewModel UpdateProfile { get; set; }
        public OrdersViewModel TrackOrders { get; set; }
        public AddressDetails1 UpdateAddress { get; set; }
        public ChangePasswordViewModel1 ChangePassword { get; set; }
    }
}
