using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Business
{
    public class StoreApp
    {
        private IStorage _db;
        private ICarts _carts;
        private ILogger<StoreApp> _logger;
        public StoreApp(IStorage db, ICarts carts, ILogger<StoreApp> logger)
        {
            _db = db;
            _carts = carts;
            _logger = logger;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            // TODO: validation for customer
            var cust = await _db.AddCustomer(customer);
            if (cust == null)
            {
                throw new Exception("Error creating customer");
            }
            return cust;
        }

        public async Task AddCustomerLogin(Customer customer, string username, string password)
        {
            await _db.AddLogin(customer, username, password);
        }

        public async Task<Product> AddProduct(Product product)
        {
            var prod = await _db.AddProduct(product);
            return prod;
        }

        public async Task<Store> AddStore(Store store)
        {
            var s = await _db.AddStore(store);
            return s;
        }

        public async Task<List<Product>> GetProducts()
        {
            var prods = await _db.GetProducts();
            return prods;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            var custs = await _db.GetCustomers();
            return custs;
        }

        public async Task<List<Product>> GetProductsByStore(Store s)
        {
            var prods = await _db.GetProducts(s);
            return prods;
        }

        public async Task<List<Product>> AddProductToInventory(Store store, Product product)
        {
            return await _db.AddStoreProduct(store, product);
        }

        public async Task<List<Product>> RemoveProductFromInventory(Store store, Product product)
        {
            // TODO: remove products from carts
            return await _db.RemoveStoreProduct(store, product);
        }

        public async Task<Customer> LoginCustomer(string username, string password)
        {
            var cust = await _db.GetLogin(username, password);
            return cust;
        }

        public async Task<Store> SelectStore(int storeId)
        {
            var s = await _db.GetStore(storeId);
            return s;
        }

        public async Task<List<Store>> GetStores()
        {
            var stores = await _db.GetStores();
            return stores;
        }

        public async Task<List<(Product, int)>> GetCart(Customer customer, Store store)
        {
            customer = await _db.GetCustomer(customer);
            store = await _db.GetStore(store.StoreId);
            var cart = _carts.GetCart(customer, store);
            return cart.GetCart();
        }

        public async Task<List<(Product, int)>> AddProductToCart(Product product, Customer customer, Store store, int quantity = 1)
        {
            customer = await _db.GetCustomer(customer);
            product = await _db.GetProduct(product);
            store = await _db.GetStore(store.StoreId);
            var storeProducts = await _db.GetProducts(store);
            var storeProduct = storeProducts.Find(sp => sp.ProductId == product.ProductId);
            if (storeProduct == null)
            {
                throw new ArgumentException("Invalid product found in cart");
            }
            var cart = _carts.GetCart(customer, store);
            _logger.LogInformation($"Getting cart for customer {customer}, id {customer.CustomerId}");
            foreach (var (p, q) in cart.GetCart())
            {
                _logger.LogInformation($"product {p.ProductId} in cart with quantity {q}");
            }
            var curQuantity = cart.GetQuantity(product);
            if (curQuantity > 0)
            {
                if (curQuantity + quantity > storeProduct.Quantity)
                {
                    throw new ArgumentException("Invalid quantity -- not enough store inventory");
                }
                cart.SetQuantity(product, curQuantity + quantity);
            }
            else
            {
                if (quantity > storeProduct.Quantity)
                {
                    throw new ArgumentException("Invalid quantity -- not enough store inventory");
                }
                cart.AddProduct(product, quantity);
            }
            _logger.LogInformation($"Checking new cart for customer {customer}, id {customer.CustomerId}");
            foreach (var (p, q) in cart.GetCart())
            {
                _logger.LogInformation($"product {p.ProductId} in cart with quantity {q}");
            }
            return cart.GetCart();
        }

        public async Task<List<(Product, int)>> RemoveProductFromCart(Product product, Customer customer, Store store, int quantity = 1)
        {
            customer = await _db.GetCustomer(customer);
            product = await _db.GetProduct(product);
            store = await _db.GetStore(store.StoreId);
            var cart = _carts.GetCart(customer, store);
            var curQuantity = cart.GetQuantity(product);
            _logger.LogInformation($"Removing quantity {quantity} of product {product} from cart of {customer} with current quantity {curQuantity}");
            if (curQuantity < quantity)
            {
                throw new ArgumentOutOfRangeException("Quantity must be less than or equal to number currently in cart");
            }
            else if (curQuantity == quantity)
            {
                cart.RemoveProduct(product);
            }
            else
            {
                cart.SetQuantity(product, curQuantity - quantity);
            }
            return cart.GetCart();
        }

        public async Task<Order> Checkout(Customer customer, Store store)
        {
            customer = await _db.GetCustomer(customer);
            store = await _db.GetStore(store.StoreId);
            var cart = _carts.GetCart(customer, store);
            var prods = cart.GetCart();
            var currentInventory = await _db.GetProducts(store);
            foreach (var (prod, quantity) in prods)
            {
                var inv = currentInventory.Find(product => product.ProductId == prod.ProductId);
                if (inv == null)
                {
                    throw new ArgumentException("Invalid product in cart");
                }
                if (inv.Quantity < quantity)
                {
                    throw new ArgumentException("Cannot purchase more products than are in store inventory");
                }
            }
            var order = await _db.CreateOrder(customer, store, prods);
            foreach (var (prod, quantity) in prods)
            {
                // remove quantity in order from store inventory
                await _db.UpdateStoreQuantity(store, prod, -quantity);
            }
            if (order != null)
            {
                cart.ClearCart();
            }
            return order;
        }

        public async Task<List<Order>> GetOrderHistory(Customer customer)
        {
            return await _db.GetOrders(customer);
        }

        public async Task<List<Order>> GetOrderHistory(Store store)
        {
            return await _db.GetOrders(store);
        }

    }
}
