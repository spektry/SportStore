using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportSore.Domain.Abstract;
using SportSore.Domain.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using SportStore.WebUI.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            //Организация
            var products = Enumerable.Range(1, 5)
                .Select(i => new Product { ProductID = i, Name = string.Format("p{0}", i) });

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products);

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            
            //Действие
            var result = (ProductListViewModel)controller.List(null, 2).Model;

            //Утверждение
            var prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("p4", prodArray[0].Name);
            Assert.AreEqual("p5", prodArray[1].Name);

            Assert.AreEqual(3, result.PagingInfo.ItemsPerPage);
            Assert.AreEqual(5, result.PagingInfo.TotalItems);
            Assert.AreEqual(2, result.PagingInfo.TotalPages);
            Assert.AreEqual(2, result.PagingInfo.CurrentPage);
        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            HtmlHelper htmlHelper = null;

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //action
            MvcHtmlString result = htmlHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>", 
                            result.ToString());
        }
    }
}
