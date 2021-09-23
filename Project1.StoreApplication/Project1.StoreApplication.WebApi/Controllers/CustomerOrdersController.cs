using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.StoreApplication.Business;
using Project1.StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.StoreApplication.WebApi.Controllers
{
    /*
    [Route("api/customers/{customerId}/orders")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {

        private StoreApp _app;

        public CustomerOrdersController(StoreApp app)
        {
            _app = app;
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders(int customerId)
        {
            var orders = await _app.GetOrderHistory(new Customer() { CustomerId = customerId });
            return orders;
        }

        [HttpPost]
        public async Task<List<Order>> AddOrder(int customerId, )

    }
    */
}
