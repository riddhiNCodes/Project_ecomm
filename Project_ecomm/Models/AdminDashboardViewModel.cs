using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ecomm.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalOrders { get; set; }

        public int TotalCustomers { get; set; }

        public decimal TotalRevenue { get; set; }

        public List<Order> RecentOrders { get; set; }
    }
}