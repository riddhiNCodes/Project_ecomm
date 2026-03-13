using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_ecomm.Models;
using Razorpay.Api;

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

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            decimal total = cart.Sum(x => x.Product.Price * x.Quantity);

            try
            {
                if (model.PaymentMethod == "Cash on Delivery")
                {
                    Models.Order order = new Models.Order
                    {
                        CustomerName = model.CustomerName,
                        Email = model.Email,
                        Phone = model.PhoneNumber,
                        Address = model.Address,
                        OrderDate = DateTime.Now,
                        TotalAmount = total,
                        Status = "Pending",
                        UserId = User.Identity.GetUserId()
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

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

                    Session["Cart"] = null;
                    ViewBag.Method = model.PaymentMethod;

                    return RedirectToAction("OrderSuccessPage", new { id = order.Id });
               
                
                }
                // If Online Payment
                if (model.PaymentMethod == "Online Payment")
                {
                    // Store order data temporarily
                    Session["OrderModel"] = model;

                    return RedirectToAction("Payment", "Order");
                }

                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong while placing order.");
                return View(model);
            }
        }
        //[HttpPost]
        //public ActionResult PlaceOrder(OrderViewModel model)
        //{
        //    var cart = Session["Cart"] as List<CartItem>;

        //    // Check if cart is empty
        //    if (cart == null || !cart.Any())
        //    {
        //        return RedirectToAction("Index", "Cart");
        //    }

        //    // Check model validation
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    try
        //    {
        //        // Create Order
        //        Models.Order order = new Models.Order
        //        {
        //            CustomerName = model.CustomerName,
        //            Email = User.Identity.Name,
        //            Phone = model.PhoneNumber,
        //            Address = model.Address,
        //            OrderDate = DateTime.Now,
        //            TotalAmount = cart.Sum(x => x.Product.Price * x.Quantity)


        //        };



        //        order.UserId = User.Identity.GetUserId();
        //        db.Orders.Add(order);
        //        order.Status = "Pending";
        //        db.SaveChanges(); // Generates Order Id

        //        // Create OrderDetails
        //        foreach (var item in cart)
        //        {
        //            OrderDetail detail = new OrderDetail
        //            {
        //                OrderId = order.Id,
        //                ProductId = item.Product.ProductId,
        //                Quantity = item.Quantity,
        //                Price = item.Product.Price
        //            };

        //            db.OrderDetails.Add(detail);
        //        }

        //        db.SaveChanges();

        //        // Clear cart
        //        Session["Cart"] = null;

        //        return RedirectToAction("OrderSuccessPage", new { id = order.Id });
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("", "Something went wrong while placing order.");
        //        return View(model);
        //    }
        //}

        public ActionResult OrderSuccessPage(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == id);


            return View(order);
        }

        [Authorize]
        public ActionResult MyOrders()
        {
            var userId = User.Identity.GetUserId();

            var orders = db.Orders
                           .Where(o => o.UserId == userId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();

            return View(orders);
        }


        public ActionResult Payment()
        {
            var cart = Session["Cart"] as List<CartItem>;

            if (cart == null || !cart.Any())
                return RedirectToAction("Index", "Cart");

            decimal total = cart.Sum(x => x.Product.Price * x.Quantity);
            int amount = Convert.ToInt32(total * 100);

            string key = ConfigurationManager.AppSettings["RazorpayKey"];
            string secret = ConfigurationManager.AppSettings["RazorpaySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>();

            options.Add("amount", amount);
            options.Add("currency", "INR");
            options.Add("receipt", Guid.NewGuid().ToString());

            Razorpay.Api.Order razorpayOrder = client.Order.Create(options);

            ViewBag.OrderId = razorpayOrder["id"].ToString();
            ViewBag.Amount = amount;
            ViewBag.Key = key;

            return View();
        }

        //public ActionResult Payment()
        //{
        //    var cart = Session["Cart"] as List<CartItem>;


        //    decimal total = cart.Sum(x => x.Product.Price * x.Quantity);

        //    string key = "rzp_test_SQkEPXWBKyFg6a";
        //    string secret = "BilFaNtQIVXClsYWi1WuiYyb";

        //    RazorpayClient client = new RazorpayClient(key, secret);

        //    Dictionary<string, object> options = new Dictionary<string, object>();

        //    options.Add("amount", Convert.ToInt32(total * 100));
        //    options.Add("currency", "INR");
        //    options.Add("receipt", Guid.NewGuid().ToString());

        //    Razorpay.Api.Order razorpayOrder = client.Order.Create(options);

        //    ViewBag.OrderId = razorpayOrder["id"].ToString();
        //    ViewBag.Amount = total * 100;
        //    ViewBag.Key = key;

        //    return View();
        //}
        public ActionResult PaymentSuccess()
        {
            var cart = Session["Cart"] as List<CartItem>;
            var model = Session["OrderModel"] as OrderViewModel;

            if (cart == null || model == null)
                return RedirectToAction("Index", "Home");

            decimal total = cart.Sum(x => x.Product.Price * x.Quantity);

            Models.Order order = new Models.Order
            {
                CustomerName = model.CustomerName,
                Email = User.Identity.Name,
                Phone = model.PhoneNumber,
                Address = model.Address,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Paid",
                UserId = User.Identity.GetUserId()
            };

            db.Orders.Add(order);
            db.SaveChanges();

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

            Session["Cart"] = null;

            return RedirectToAction("OrderSuccessPage", new { id = order.Id });
        }


    }
}