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
                // Save order in database here

                Session["Cart"] = null;

                return RedirectToAction("OrderSuccess");
            }

            return View(model);
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}