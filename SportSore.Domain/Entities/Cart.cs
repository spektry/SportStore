using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportSore.Domain.Entities
{
    public class Cart
    {
        private readonly List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        #region CRUD operation

        public void AddLine(Product p, int quantity)
        {
            if (p == null)
                throw new ArgumentNullException("p");

            CartLine line = lineCollection
                .Where(pline => pline.Product.ProductID == p.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                var newLIne = new CartLine() { Product = p, Quantity = quantity };
                lineCollection.Add(newLIne);
            }
            else
                line.Quantity += quantity;
        }

        public void RemoveLine(Product p)
        {
            if (p == null)
                throw new ArgumentNullException("p");

            lineCollection.RemoveAll(l => l.Product.ProductID == p.ProductID);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        #endregion

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(l => l.Product.Price * l.Quantity);
        }
    }

    public class CartLine
    {        
        public Product Product { get; set; }
        public int Quantity { get; set; } // Сколько шт.
    }
}
