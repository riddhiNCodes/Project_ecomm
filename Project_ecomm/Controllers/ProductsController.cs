using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_ecomm.Models;

namespace Project_ecomm.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();



        // GET: Produts

        public ActionResult Index(string category)
        {
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            return View(products.ToList());
        }


        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            var existingItem = cart.FirstOrDefault(c => c.Product.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }

            Session["Cart"] = cart;

            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return View(cart);
        }


        public ActionResult IncreaseQty(int id)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            var item = cart.FirstOrDefault(c => c.Product.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }

            Session["Cart"] = cart;
            return RedirectToAction("Cart");
        }

        public ActionResult DecreaseQty(int id)
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            var item = cart.FirstOrDefault(c => c.Product.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            Session["Cart"] = cart;
            return RedirectToAction("Cart");
        }


        public ActionResult Checkout()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Cart");
            }

            return View(cart);
        }
    }
}