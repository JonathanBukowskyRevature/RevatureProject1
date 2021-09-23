using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Storage.DBModels
{
    public class DBCustomerLogin
    {
        public int CustomerLoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CustomerId { get; set; }
        public DBCustomer Customer { get; set; }
    }
}
