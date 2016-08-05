using SportSore.Domain.Abstract;
using SportSore.Domain.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            if (orderProcessor == null)
                throw new ArgumentNullException("orderProcessor");

            this.repository = repository;
            this.orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            var zViewModel = new CartIndexViewModel()
            {
                Cart = cart,
                ReturnUrl = returnUrl
            };

            return View("Index", zViewModel);
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
                cart.AddLine(product, 1);

            return RedirectToAction("Index", "Cart", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", "Cart", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
	}
}