using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Project1.StoreApplication.Storage;
using Project1.StoreApplication.Models;
using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Storage.DBConverters;
using Project1.StoreApplication.Business;
using Microsoft.Extensions.Logging;

namespace Project1.StoreApplication.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private StoreApp _app;
        private ILogger<CustomersController> _logger;
        /*
        public Customers() : base()
        {
            _db = new StoreApplicationDB2Context();
        }
        */
        public CustomersController(StoreApp app, ILogger<CustomersController> logger) : base()
        {
            _app = app;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            //var custs = _db.Customers.FromSqlRaw("SELECT * FROM Customer.Customer");
            //var result = await (from c in custs select c.ConvertToModel()).ToListAsync();
            var result = await _app.GetCustomers();
            return result;
        }

        [HttpGet("{customerId}/cart/{storeId}")]
        public async Task<List<object>> GetCart(int customerId, int storeId)
        {
            var result = await _app.GetCart(new Customer() { CustomerId = customerId }, new Store() { StoreId = storeId });
            return ConvertToJSON(result);
        }

        public class CartSubmissionData
        {
            public int storeId { get; set; }
            public int productId { get; set; }
        }

        private static List<object> ConvertToJSON(List<(Product, int)> data)
        {
            var result = new List<object>();
            foreach (var (product, quantity) in data)
            {
                result.Add(new { product, quantity });
            }
            return result;
        }

        [HttpPost("{customerId}/cart/{storeId}/add")]
        public async Task<List<object>> AddToCart(int customerId, int storeId, [FromBody] CartSubmissionData data)
        {
            // TODO: make this work right for selected stores
            var cart = await _app.AddProductToCart(
                new Product() { ProductId = data.productId },
                new Customer() { CustomerId = customerId },
                new Store() { StoreId = storeId }
            );
            return ConvertToJSON(cart);
        }

        [HttpPost("{customerId}/cart/{storeId}/remove")]
        public async Task<List<object>> RemoveFromCart(int customerId, int storeId, [FromBody] CartSubmissionData data)
        {
            var cart = await _app.RemoveProductFromCart(
                new Product() { ProductId = data.productId },
                new Customer() { CustomerId = customerId },
                new Store() { StoreId = storeId }
            );
            return ConvertToJSON(cart);
        }

        public class CreateCustomerData
        {
            public string username { get; set; }
            public string password { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<Customer> AddCustomer([FromBody] CreateCustomerData customer)
        {
            var cust = new Customer() { FirstName = customer.firstName, LastName = customer.lastName };
            //TODO: Validation
            var addedCust = await _app.AddCustomer(cust);
            await _app.AddCustomerLogin(addedCust, customer.username, customer.password);
            return addedCust;
        }

        [HttpGet("{customerId}/orders")]
        public async Task<List<Order>> GetOrders(int customerId)
        {
            var orders = await _app.GetOrderHistory(new Customer() { CustomerId = customerId });
            return orders;
        }

        [HttpPost("{customerId}/checkout/{storeId}")]
        public async Task<Order> Checkout(int customerId, int storeId)
        {
            _logger.LogInformation($"Checking out customer {customerId} from store {storeId}...");
            var order = await _app.Checkout(new Customer() { CustomerId = customerId }, new Store() { StoreId = storeId });
            _logger.LogInformation($"Recieved order {order.OrderID} from checkout operation");
            return order;
        }

    }
}
