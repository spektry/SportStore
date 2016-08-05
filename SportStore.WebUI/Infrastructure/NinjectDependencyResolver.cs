using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using SportSore.Domain.Abstract;
using Moq;
using SportSore.Domain.Entities;
using SportSore.Domain.Concrete;
using System.Configuration;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentException("kernel == null");

            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //привязки:
            //kernel.Bind<IProductRepository>().ToConstant(GetFakeProductRepository());
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            //Доставка
            var emailSettings = new EmailSettings()
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("emailSettings", emailSettings);
        }

        #region # FakeProductRepository
        private IProductRepository GetFakeProductRepository()
        {
            var fakeProducts = new List<Product> {
                new Product { Name = "Football", Price = 25},
                new Product { Name = "Surf board", Price = 179},
                new Product { Name = "Running shoes", Price = 95}
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(m => m.Products).Returns(fakeProducts);

            return mockRepository.Object;
        }
        #endregion
        
    }
}