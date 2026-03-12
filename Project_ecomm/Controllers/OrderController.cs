using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_ecomm.Models;
using Microsoft.AspNet.Identity;

namespace Project_ecomm.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
       private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
        public ActionResult PlaceOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = Session["Cart"] as List<CartItem>;

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            OrderViewModel model = new OrderViewModel
            {
                CartItems = cart
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult PlaceOrder(OrderViewModel model)
        {
            var cart = Session["Cart"] as List<CartItem>;

            // Check if cart is empty
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Check model validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Create Order
                Order order = new Order
                {
                    CustomerName = model.CustomerName,
                    Email = model.Email,
                    Phone = model.PhoneNumber,
                    Address = model.Address,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.Sum(x => x.Product.Price * x.Quantity)
                
                
                };
                order.UserId = User.Identity.GetUserId();
                db.Orders.Add(order);
                order.Status = "Pending";
                db.SaveChanges(); // Generates Order Id

                // Create OrderDetails
                foreach (var item in cart)
                {
                    OrderDetail detail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.Product.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };

                    db.OrderDetails.Add(detail);
                }

                db.SaveChanges();

                // Clear cart
                Session["Cart"] = null;

                return RedirectToAction("OrderSuccessPage", new { id = order.Id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong while placing order.");
                return View(model);
            }
        }


        public ActionResult OrderSuccessPage(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == id);

            return View(order);
        }


        [Authorize]
        public ActionResult MyOrders()
        {
            string userEmail = User.Identity.GetUserName();

            var orders = db.Orders
                           .Where(o => o.Email == userEmail)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();

            return View(orders);
        }
    }
}