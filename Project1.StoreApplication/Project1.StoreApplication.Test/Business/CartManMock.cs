using Project1.StoreApplication.Business;
using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Test.Business
{
    public class CartManMock : ICarts
    {
        public ICart GetCart(Customer customer, Store store)
        {
            if (customer.CustomerId > 0 && customer.CustomerId < 5
                && store.StoreId > 0 && store.StoreId < 5)
            {
                return new CartMock(false);
            }
            // return an empty cart
            return new CartMock(true);
        }
    }
}
