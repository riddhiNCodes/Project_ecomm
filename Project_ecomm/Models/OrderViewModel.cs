using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_ecomm.Models
{
    public class OrderViewModel
    {
        public List<CartItem> CartItems { get; set; }

      
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }


}