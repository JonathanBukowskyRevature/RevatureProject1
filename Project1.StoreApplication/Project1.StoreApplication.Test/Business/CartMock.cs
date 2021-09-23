using Project1.StoreApplication.Business;
using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Test.Business
{
    public class CartMock : ICart
    {
        private bool _empty;
        public CartMock(bool empty)
        {
            _empty = empty;
        }

        public void AddProduct(Product product, int quantity)
        {
            throw new NotImplementedException();
        }

        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public List<(Product, int)> GetCart()
        {
            if (_empty)
            {
                return new();
            }

            return new()
            {
                (new Product()
                {
                    ProductId = 1,
                    Name = "Product 1",
                    Description = "prod 1",
                    Price = 1.99m,
                    Quantity = 1
                }, 1),
                (new Product()
                {
                    ProductId = 2,
                    Name = "Product 2",
                    Description = "prod 2",
                    Price = 2.99m,
                    Quantity = 2
                }, 2),
                (new Product()
                {
                    ProductId = 3,
                    Name = "Product 3",
                    Description = "prod 3",
                    Price = 3.99m,
                    Quantity = 3
                }, 3),
            };
        }

        public int GetQuantity(Product product)
        {
            var prods = GetCart().ConvertAll(((Product product, int q) t) => product);
            var prod = prods.Find((p) => p.ProductId == product.ProductId);
            if (prod != null)
            {
                return prod.Quantity;
            } else
            {
                return 0;
            }
        }

        public void RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void SetQuantity(Product product, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
