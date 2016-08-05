using SportSore.Domain.Abstract;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize { get; set; }
        private const int defaultItemsInPage = 3; 

        public ProductController(IProductRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            this.repository = repository;
            this.PageSize = defaultItemsInPage;
        }

        public ViewResult List(string category, int page = 1)
        {
            var productsByCategory = repository.Products
                .Where(p => category == null || p.Category == category)
                .ToList();

            var productsInPage = productsByCategory
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = productsByCategory.Count
            };

            var listModel = new ProductListViewModel()
            {
                Products = productsInPage,
                PagingInfo = pagingInfo,
                CurrentCategory = category
            };

            return View(listModel);
        }
	}
}