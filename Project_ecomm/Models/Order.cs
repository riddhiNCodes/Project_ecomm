using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ecomm.Models
{
        public class Order
        {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public int Id { get; set; }
            public string CustomerName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        }
    }
