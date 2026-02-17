using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_ecomm.Models;

namespace Project_ecomm.Controllers
{

        [Authorize] 
        public class AdminController : Controller
        {
            ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }



        // GET: Add Product
        public ActionResult AddProducts()
            {
                return View();
            }

            // POST: Add Product
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AddProducts(Product product, HttpPostedFileBase ImageFile)
            {
                if (ModelState.IsValid)
                {
                    // Image upload
                    if (ImageFile != null)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                        ImageFile.SaveAs(path);

                        product.ImageUrl = "/Images/Products/" + fileName;
                    }

                    product.CreatedDate = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();

                    ViewBag.Success = "Product added successfully!";
                    ModelState.Clear();
                }

                return View();
            }
        public ActionResult ProductList()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        //Get Edit Product
        public ActionResult EditProduct(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Edit Product
        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase ImageFile)
        {
            var product = db.Products.Find(model.ProductId);

            if (product != null)
            {
                product.Name = model.Name;
                product.Category = model.Category;
                product.Price = model.Price;
                product.Description = model.Description;

                // Image Upload
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    ImageFile.SaveAs(path);

                    product.ImageUrl = "~/Images/" + fileName;
                }

                db.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = db.Products.Find(id);

            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }

    }
}
