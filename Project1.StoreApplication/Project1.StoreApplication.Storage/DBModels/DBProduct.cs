using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBProduct : DBObject
    {
        public DBProduct()
        {
            OrderProducts = new HashSet<DBOrderProduct>();
            StoreProducts = new HashSet<DBStoreProduct>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public bool? Active { get; set; }

        public virtual DBProductCategory Category { get; set; }
        public virtual ICollection<DBOrderProduct> OrderProducts { get; set; }
        public virtual ICollection<DBStoreProduct> StoreProducts { get; set; }
    }
}
