using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Business;
using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.StoreApplication.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private StoreApp _app;
        private ILogger<StoresController> _logger;
        public StoresController(StoreApp app, ILogger<StoresController> logger)
        {
            _app = app;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<Store>> GetStores()
        {
            return await _app.GetStores();
        }

        [HttpGet("{storeId}/products")]
        public async Task<List<Product>> GetProducts(int storeId)
        {
            var products = await _app.GetProductsByStore(new Store() { StoreId = storeId });
            return products;
        }

        [HttpPost("{storeId}/products/add")]
        [Consumes("application/json")]
        public async Task<List<Product>> AddProductToInventory(int storeId, [FromBody] Product product)
        {
            _logger.LogInformation($"addProduct, storeId: {storeId}, Product given: {product}");
            try
            {
                var newInventory = await _app.AddProductToInventory(new Store() { StoreId = storeId }, product);
                return newInventory;
            }
            catch (SqlException e)
            {
                // TODO: Better error handling
                if (e.Message.Contains("UNIQUE"))
                {
                    _logger.LogInformation($"Tried to add a duplicate product {product.ProductId} to store {storeId} inventory");
                    var newInventory = await GetProducts(storeId);
                    return newInventory;
                }
                return null;
            }
        }

        [HttpPost("{storeId}/products/remove")]
        [Consumes("application/json")]
        public async Task<List<Product>> RemoveProductFromInventory(int storeId, [FromBody] Product product)
        {
            var newInventory = await _app.RemoveProductFromInventory(new Store() { StoreId = storeId }, product);
            return newInventory;
        }

        [HttpPost("select/{storeId}")]
        [Consumes("application/json")]
        public async Task<object> SelectStore(int storeId)
        {
            var store = await _app.SelectStore(storeId);
            if (store == null)
            {
                return new { success = false, store = (Store) null };
            }
            return new { success = true, store };
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<Store> AddStore(Store store)
        {
            return await _app.AddStore(store);
        }

        [HttpGet("{storeId}/orders")]
        public async Task<List<Order>> GetOrders(int storeId)
        {
            var orders = await _app.GetOrderHistory(new Store() { StoreId = storeId });
            return orders;
        }
    }
}
