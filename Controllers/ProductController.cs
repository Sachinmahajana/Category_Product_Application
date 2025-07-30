using CategoryApplication.Data;
using CategoryApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CategoryApplication.Controllers
{
    public class ProductController:Controller
    {
        private AppDbContext db = new AppDbContext();

        //public ActionResult Index(int page = 1, int pageSize = 5)
        //{
        //    var products = db.Products.Include(p => p.Category)
        //                        .OrderBy(p => p.ProductId)
        //                        .Skip((page - 1) * pageSize)
        //                        .Take(pageSize)
        //                        .ToList();

        //    ViewBag.Page = page;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.TotalCount = db.Products.Count();

        //    return View(products);
        //}
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var products = db.Products.Include(p => p.Category)
                                      .OrderBy(p => p.ProductId)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = db.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / pageSize);
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View(product);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}