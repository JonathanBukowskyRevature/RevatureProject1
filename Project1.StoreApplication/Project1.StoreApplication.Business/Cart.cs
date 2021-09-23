using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Business
{

    // TODO: I don't think this is safe for concurrency
    public class Cart : ICart
    {
        private Dictionary<Product, int> Products;
        public Cart()
        {
            Products = new();
        }

        /// <summary>
        /// Add product to cart with initial quantity
        /// </summary>
        /// <param name="product">product to add to cart</param>
        /// <param name="quantity">Number of products to add to cart</param>
        public void AddProduct(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("Quantity must be positive integer");
            }
            if (Products.ContainsKey(product))
            {
                throw new ArgumentException("Product must not exist in cart");
            }
            Products[product] = quantity;
        }

        /// <summary>
        /// Get products and quantities in cart
        /// </summary>
        /// <returns>List of (Product p, int quantity) tuples</returns>
        public List<(Product, int)> GetCart()
        {
            return (from p in Products select (p.Key, p.Value)).ToList();
        }

        /// <summary>
        /// Return number of products product in cart (0 if none exist)
        /// </summary>
        /// <param name="product">Product for which to get quantity</param>
        /// <returns></returns>
        public int GetQuantity(Product product)
        {
            if (!Products.ContainsKey(product))
            {
                return 0;
            }
            return Products[product];
        }

        /// <summary>
        /// Update the quantity of a product already in the cart
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void SetQuantity(Product product, int quantity)
        {
            if (!Products.ContainsKey(product))
            {
                throw new ArgumentException("Product must exist in cart");
            }
            Products[product] = quantity;
        }

        /// <summary>
        /// Decrease quantity
        /// </summary>
        /// <param name="product"></param>
        public void RemoveProduct(Product product)
        {
            if (!Products.ContainsKey(product))
            {
                throw new ArgumentException("Product must exist in cart");
            }
            Products.Remove(product);
        }

        public void ClearCart()
        {
            Products.Clear();
        }
    }
}
