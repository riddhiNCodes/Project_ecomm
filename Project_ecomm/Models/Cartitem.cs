using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ecomm.Models
{
    public class CartItem
    {
        public Product Product { get; set; }

        public string proname { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        //public decimal? DiscountPrice { get; set; }
        public string ImageUrl { get; set; }

        public string Category { get; set; }   // Bangles, Earrings, Necklace etc.

      public int Quantity { get; set; }

      
    }

}