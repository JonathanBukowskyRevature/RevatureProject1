using System;
using System.Collections.Generic;
//using System.Xml.Serialization;

namespace Project1.StoreApplication.Models
{
    /// <summary>
    /// Store class - objects to represent a single store in our application
    /// </summary>
    public class Store
    {
        // TODO: Reconsider which constructors are appropriate
        public Store()
        {
            Orders = new();
        }
        public Store(string name)
        {
            Name = name;
            Orders = new();
        }
        public string Name { get; set; }

        public int StoreId { get; set; }

        /// <summary>
        /// The list of orders that are associated with this store
        /// </summary>
        /// <value></value>
        public List<Order> Orders { get; set; }

        public override int GetHashCode()
        {
            return StoreId;
        }
        public override bool Equals(object o)
        {
            if (o is Store)
            {
                return (this == (o as Store));
            }
            return false;
        }
        public static bool operator !=(Store a, Store b)
        {
            return a?.StoreId != b?.StoreId;
        }
        public static bool operator ==(Store a, Store b)
        {
            return a?.StoreId == b?.StoreId;
        }

        public override string ToString()
        {
            return Name ?? DateTime.Now.ToLongDateString();
        }
    }
}