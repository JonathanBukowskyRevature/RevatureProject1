using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Project1.StoreApplication.Models
{
    /// <summary>
    /// An Order created in StoreApplication will be an already confirmed, non-modifiable order placed by a Customer at a Store
    /// </summary>
    public class Order
    {
        // TODO: I'm not sure I want this here.
        public Order() : base()
        {

        }
        public Order(Customer customer, Store store, List<Product> products) : base()
        {
            Customer = customer;
            Products = new(products);
            Store = store;
        }

        // TODO: update OrderID to be set appropriately
        public int OrderID { get; set; }

        /// <summary>
        /// Customer placing the order
        /// </summary>
        /// <value></value>
        public Customer Customer { get; set; }

        /// <summary>
        /// Products being ordered
        /// </summary>
        /// <value></value>
        [NotMapped]
        public List<Product> Products { get; set; }

        /// <summary>
        /// Get list of products in order
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            // TODO: return a copy of products, or make this readonly somehow (maybe the readonly collection)
            return Products;
        }

        /// <summary>
        /// Add a product to order
        /// </summary>
        /// <param name="product">Product to add to order</param>
        /// <returns>Error string, or null if successful</returns>
        public string AddProduct(Product product)
        {
            // TODO: add error checking (can only have 50 items and $500 in an order) and return appropriately
            Products.Add(product);
            return null;
        }

        /// <summary>
        /// Remove product from order
        /// </summary>
        /// <param name="product">Product to remove from order</param>
        /// <returns>Error string, or null if successful</returns>
        public string RemoveProduct(Product product)
        {
            if (!Products.Remove(product))
            {
                return "Failed to remove product from order";
            }
            return null;
        }

        /// <summary>
        /// Get total price for the order
        /// </summary>
        /// <returns>total cost of all products</returns>
        public decimal GetTotal()
        {
            decimal sum = 0;
            Products.ForEach(p => sum += p.Price);
            return sum;
        }

        /// <summary>
        /// Store through which order is being made
        /// </summary>
        /// <value></value>
        public Store Store { get; set; }

        public override int GetHashCode()
        {
            return OrderID;
        }
        public override bool Equals(object o)
        {
            if (o is Order)
            {
                return (this == (o as Order));
            }
            return false;
        }

        public static bool operator !=(Order a, Order b)
        {
            return a?.OrderID != b?.OrderID;
        }
        public static bool operator ==(Order a, Order b)
        {
            return a?.OrderID == b?.OrderID;
        }

        public override string ToString()
        {
            string output = $"Order:\t${GetTotal():f2}\n\tCustomer: " + Customer + "\n\tStore: " + Store + "\n\tItems:\n";
            foreach (var product in Products)
            {
                output += "\t\t" + product + "\n";
            }
            return output;
        }
    }
}