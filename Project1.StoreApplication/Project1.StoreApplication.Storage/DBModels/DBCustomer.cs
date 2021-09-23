using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBCustomer : DBObject
    {
        public DBCustomer()
        {
            Orders = new HashSet<DBOrder>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DefaultStore { get; set; }
        public bool? Active { get; set; }

        public virtual DBStore DefaultStoreNavigation { get; set; }
        public virtual DBCustomerLogin CustomerLogin { get; set; }
        public virtual ICollection<DBOrder> Orders { get; set; }
    }
}
