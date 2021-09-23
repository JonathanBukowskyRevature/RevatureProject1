using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBOrderProduct : DBObject
    {
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool? Active { get; set; }

        public virtual DBOrder Order { get; set; }
        public virtual DBProduct Product { get; set; }
    }
}
