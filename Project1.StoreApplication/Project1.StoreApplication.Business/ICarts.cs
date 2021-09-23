using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Business
{
    public interface ICarts
    {
        ICart GetCart(Customer customer, Store store);
    }
}
