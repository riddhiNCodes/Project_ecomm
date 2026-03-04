using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_ecomm.Models;

namespace Project_ecomm.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult PlaceOrder()
        {
            var cart = Session["Cart"] as List<CartItem>;

            OrderViewModel model = new OrderViewModel
            {
                CartItems = cart
            };

            return View(model);
        }

        [HttpPost]
      
        public ActionResult PlaceOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1️⃣ Create order object
                Order order = new Order();
                order.CustomerName = model.CustomerName;
                order.Address = model.Address;
                order.TotalAmount = model.TotalAmount;
                order.OrderDate = DateTime.Now;

                // 2️⃣ Save in database
                db.Orders.Add(order);
                db.SaveChanges();   // VERY IMPORTANT (this generates Order ID)

                // 3️⃣ Clear cart
                Session["Cart"] = null;

                // 4️⃣ Redirect and pass order ID
                return RedirectToAction("OrderSuccess", new { id = order.Id });
            }

            return View(model);
        }
        

        public ActionResult OrderSuccessPage(int id)
        {
            ViewBag.OrderId = id;
            return View();

        }
    }
}