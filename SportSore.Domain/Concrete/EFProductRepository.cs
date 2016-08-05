using SportSore.Domain.Abstract;
using SportSore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportSore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private readonly EFDbContext dbContext = new EFDbContext();

        public IEnumerable<Entities.Product> Products
        {
            get { return dbContext.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            if (product.ProductID == 0)
            {
                dbContext.Products.Add(product);
            }
            else
            {
                Product dbEntry = dbContext.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }

            dbContext.SaveChanges();
        }
    }
}
