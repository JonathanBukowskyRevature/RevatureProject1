using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Business
{
    public class CartManager<T> : ICarts where T : ICart, new()
    {
        // TODO: Use interface instead of concrete class?
        private Dictionary<(Customer, Store), T> _carts;
        public CartManager()
        {
            _carts = new();
        }

        public ICart GetCart(Customer customer, Store store)
        {
            var key = (customer, store);
            if (!_carts.ContainsKey(key))
            {
                _carts[key] = new T();
            }
            return _carts[key];
        }
    }
}
