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
        }
    }
