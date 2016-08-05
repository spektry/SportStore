using SportSore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            this.repository = repository;
        }

        public PartialViewResult Menu(string category = null, bool horizontalLayout = false)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categoryList = repository.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(x => x);

            return PartialView("FlexMenu", categoryList);
        }
	}
}