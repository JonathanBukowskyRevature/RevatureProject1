using System;
using System.Collections.Generic;

#nullable disable

namespace Project1.StoreApplication.Storage.DBModels
{
    public partial class DBProductCategory : DBObject
    {
        public DBProductCategory()
        {
            Products = new HashSet<DBProduct>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<DBProduct> Products { get; set; }
    }
}
