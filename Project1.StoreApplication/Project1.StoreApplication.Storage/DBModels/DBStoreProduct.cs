using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBStoreProduct : DBObject
    {
        public int StoreProductId { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool? Active { get; set; }

        public virtual DBProduct Product { get; set; }
        public virtual DBStore Store { get; set; }
    }
}
