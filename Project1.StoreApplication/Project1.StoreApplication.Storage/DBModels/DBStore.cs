using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBStore : DBObject
    {
        public DBStore()
        {
            Customers = new HashSet<DBCustomer>();
            Orders = new HashSet<DBOrder>();
            StoreProducts = new HashSet<DBStoreProduct>();
        }

        public int StoreId { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<DBCustomer> Customers { get; set; }
        public virtual ICollection<DBOrder> Orders { get; set; }
        public virtual ICollection<DBStoreProduct> StoreProducts { get; set; }
    }
}
