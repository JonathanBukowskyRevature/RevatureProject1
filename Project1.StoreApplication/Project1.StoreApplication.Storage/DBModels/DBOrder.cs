using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBOrder : DBObject
    {
        public DBOrder()
        {
            OrderProducts = new HashSet<DBOrderProduct>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool? Active { get; set; }

        public virtual DBCustomer Customer { get; set; }
        public virtual DBStore Store { get; set; }
        public virtual ICollection<DBOrderProduct> OrderProducts { get; set; }
    }
}
