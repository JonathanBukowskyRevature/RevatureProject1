using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage.DBModels;

namespace Project1.StoreApplication.Storage.DBConverters
{
    public static class ModelConverterExtensions
    {
        /*

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DefaultStore { get; set; }
        public bool? Active { get; set; }

        public virtual Store DefaultStoreNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

         */
        /// <summary>
        /// Converts a DBObject Customer into a ModelCustomer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static Customer ConvertToModel(this DBCustomer customer)
        {
            if (customer == null)
                return null;
            Customer cust = new();
            cust.CustomerId = customer.CustomerId;
            cust.FirstName = customer.FirstName;
            cust.LastName = customer.LastName;
            // TODO: find a way to do this without circular logic
            /*
            cust.Orders = (from o in customer.Orders
                          select o.ConvertToModel()).ToList();
            */
            return cust;
        }

        public static Order ConvertToModel(this DBOrder order)
        {
            if (order == null)
                return null;
            Order o = new();
            o.OrderID = order.OrderId;
            o.Products = new();
            if (order.OrderProducts != null)
            {
                foreach (var op in order.OrderProducts)
                {
                    if (op.Product != null)
                    {
                        var prod = op.Product.ConvertToModel();
                        prod.Quantity = op.Quantity;
                        o.Products.Add(prod);
                    }
                }
            }
            o.Store = order.Store.ConvertToModel();
            o.Customer = order.Customer.ConvertToModel();
            return o;
        }

        public static Store ConvertToModel(this DBStore store)
        {
            if (store == null)
                return null;
            Store s = new();
            s.StoreId = store.StoreId;
            s.Name = store.Name;
            // TODO: find a way to do this without circular logic
            /*
            s.Orders = (from o in store.Orders select o.ConvertToModel()).ToList();
            */
            return s;
        }

        public static Product ConvertToModel(this DBProduct product)
        {
            if (product == null)
                return null;
            Product p = new();
            p.ProductId = product.ProductId;
            p.Name = product.Name;
            p.Description = product.Description;
            p.CategoryID = product.CategoryId;
            p.Price = product.Price;
            return p;
        }

        // TODO: I might not need these functions, but if I use them, I need to make
        //  sure that I'm not duplicating objects or something dumb like that

        public static DBCustomer ConvertToDBObj(this Customer customer)
        {
            if (customer == null)
                return null;
            DBCustomer c = new();
            c.CustomerId = customer.CustomerId;
            c.FirstName = customer.FirstName;
            c.LastName = customer.LastName;
            return c;
        }

        public static DBOrder ConvertToDBObj(this Order order)
        {
            if (order == null)
                return null;
            DBOrder o = new();
            // TODO: give o values
            return o;
        }

        public static DBStore ConvertToDBObj(this Store store)
        {
            if (store == null)
                return null;
            DBStore s = new();
            // TODO: give s values
            return s;
        }

        public static DBProduct ConvertToDBObj(this Product product)
        {
            if (product == null)
                return null;
            DBProduct p = new();
            // TODO: give p values
            return p;
        }
    }
}
