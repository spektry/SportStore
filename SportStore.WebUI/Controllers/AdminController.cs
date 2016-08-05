using SportSore.Domain.Abstract;
using SportSore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            this.repository = repository;
        }

        public ActionResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("'{0}' сохранен", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так с данными
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }
	}
}