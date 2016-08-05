using SportSore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Infrastructure
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;

            var session = controllerContext.HttpContext.Session;
            if (session != null)
                cart = (Cart)session[sessionKey];

            if (cart == null)
            {
                cart = new Cart();
                if (session != null)
                    session[sessionKey] = cart;
            }

            return cart;
        }
    }
}