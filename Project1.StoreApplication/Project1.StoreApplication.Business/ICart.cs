using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Business
{
    public interface ICart
    {
        void AddProduct(Product product, int quantity);
        List<(Product, int)> GetCart();
        int GetQuantity(Product product);
        void SetQuantity(Product product, int quantity);
        void RemoveProduct(Product product);
        void ClearCart();
    }
}
