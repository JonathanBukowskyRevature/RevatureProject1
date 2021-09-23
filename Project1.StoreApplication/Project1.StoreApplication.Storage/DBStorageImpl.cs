using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage.DBModels;
using Project1.StoreApplication.Storage.DBConverters;
using Microsoft.Extensions.Logging;

namespace Project1.StoreApplication.Storage
{
    public class DBStorageImpl : IStorage
    {
        private readonly StoreApplicationDB2Context _db;
        private readonly ILogger<DBStorageImpl> _logger;
        /*
        public DBStorageImpl() : base()
        {
            _db = new StoreApplicationDB2Context();
        }
        */
        public DBStorageImpl(StoreApplicationDB2Context db, ILogger<DBStorageImpl> logger) : base()
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Order> CreateOrder(Customer customer, Store store, List<(Product, int)> products)
        {
            // TODO: validation
            if (customer == null)
            {
                return null;
            }
            if (store == null)
            {
                return null;
            }
            if (products == null)
            {
                return null;
            }
            // TODO: just use CURRENT_TIMESTAMP? I originally added this in because we had a constraint to check that orderdate <= GETTIME(),
            //  and making the order 1 min in the past was fixing that, but I don't think that's a good solution
            _logger.LogInformation($"Creating order for user {customer.CustomerId} at store {store.StoreId}");
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO Store.[Order] (CustomerID, StoreID, OrderDate)" +
                " VALUES ({0}, {1}, DATEADD(minute, -1, CURRENT_TIMESTAMP))", customer.CustomerId, store.StoreId);
            await _db.SaveChangesAsync();
            // TODO: Check for success/failure
            // TODO: this is not safe for concurrent use
            var result = await _db.Orders.FromSqlRaw(
                    "SELECT TOP(1) * FROM Store.[Order] WHERE CustomerID={0} AND StoreID={1} ORDER BY OrderDate DESC",
                    customer.CustomerId,
                    store.StoreId
                ).FirstAsync();
            if (result == null)
            {
                throw new DbUpdateException("Error saving order to db");
            }
            _logger.LogInformation($"Saved order to databse with id {result.OrderId}");
            foreach (var (product, quantity) in products)
            {
                _logger.LogInformation($"Saving product {product.ProductId} to order {result.OrderId} with quantity {quantity}");
                _db.Database.ExecuteSqlRaw("INSERT INTO Store.OrderProduct (OrderID, ProductID, Quantity) VALUES ({0}, {1}, {2})",
                    result.OrderId,
                    product.ProductId,
                    quantity
                );
                _logger.LogInformation($"Product {product.ProductId} inserted");
            }
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Order data saved, attaching products to result");
            await AttachProductsToOrder(result);
            _logger.LogInformation($"Converting order object to model");
            var orderModel = result.ConvertToModel();
            _logger.LogInformation($"Done converting, got orderModel for order {orderModel.OrderID}, with {orderModel.Products.Count} products");
            return orderModel;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            var custs = await _db.Customers.FromSqlRaw("SELECT * FROM Customer.Customer").ToListAsync();
            return custs.ConvertAll(c => c.ConvertToModel());
        }

        public async Task<Customer> GetCustomer(Customer customer)
        {
            var cust = await _db.Customers.FromSqlRaw("SELECT * FROM Customer.Customer WHERE CustomerID = {0}", customer.CustomerId).FirstOrDefaultAsync();
            if (cust == null)
            {
                throw new ArgumentException("Invalid customer");
            }
            return cust.ConvertToModel();
        }


        public async Task AddLogin(Customer customer, string username, string password)
        {
            if (username == "")
            {
                throw new ArgumentException("Username must have a value");
            }
            if (password == "")
            {
                throw new ArgumentException("Password must have a value");
            }
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO Customer.CustomerLogin (Username, Password, CustomerID) VALUES ({0}, {1}, {2})",
                username,
                password,
                customer.CustomerId
            );
        }

        public async Task<Customer> GetLogin(string username, string password)
        {
            var login = await _db.CustomerLogins.FromSqlRaw("SELECT * FROM Customer.CustomerLogin WHERE Username = {0} AND Password = {1}",
                username, password).FirstOrDefaultAsync();
            if (login == null)
            {
                // TODO: No logins found, could return error
                return null;
            }
            var cust = await _db.Customers.FromSqlRaw("SELECT * FROM Customer.Customer WHERE CustomerID = {0}", login.CustomerId).FirstAsync();
            return cust.ConvertToModel();
        }


        private static readonly string _orderSubQueryOrderProducts =
            "SELECT op.* " +
            "FROM Store.OrderProduct AS op " +
            "WHERE op.OrderID = {0}";
        private static readonly string _orderProductSubQuery =
            "SELECT * FROM Store.Product WHERE ProductID = {0}";
        private async Task AttachProductsToOrder(DBOrder order)
        {
            _logger.LogInformation($"Getting orderProducts for order {order.OrderId}");
            order.OrderProducts = await _db.OrderProducts.FromSqlRaw(_orderSubQueryOrderProducts, order.OrderId).ToListAsync();
            _logger.LogInformation($"{order.OrderProducts.Count} orderProducts found for order {order.OrderId}");
            foreach (var op in order.OrderProducts)
            {
                _logger.LogInformation($"Getting product for orderproduct {op.OrderProductId}, to replace {op.Product}");
                op.Product = await _db.Products.FromSqlRaw(_orderProductSubQuery, op.ProductId).FirstAsync();
                _logger.LogInformation($"Got product information for product {op.Product.ProductId}");
            }
            _logger.LogInformation("Done attaching products to order object");
        }

        private async Task AttachOrderInfo(DBOrder order)
        {
            order.Customer = await _db.Customers.FromSqlRaw("SELECT * FROM Customer.Customer WHERE CustomerID = {0}", order.CustomerId).FirstAsync();
            order.Store = await _db.Stores.FromSqlRaw("SELECT * FROM Store.Store WHERE StoreID = {0}", order.StoreId).FirstAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            var ords = await _db.Orders.FromSqlRaw("SELECT * FROM Store.[Order]").ToListAsync();
            foreach (var order in ords)
            {
                await AttachProductsToOrder(order);
                await AttachOrderInfo(order);
            }
            return ords.ConvertAll(o => o.ConvertToModel());
        }

        public async Task<List<Order>> GetOrders(Store store)
        {
            var ords = await _db.Orders.FromSqlRaw("SELECT * FROM Store.[Order] WHERE StoreID={0}", store.StoreId).ToListAsync();
            foreach (var order in ords)
            {
                await AttachProductsToOrder(order);
                await AttachOrderInfo(order);
            }
            return ords.ConvertAll(o => o.ConvertToModel());
        }

        public async Task<List<Order>> GetOrders(Customer customer)
        {
            var ords = await _db.Orders.FromSqlRaw("SELECT * FROM Store.[Order] WHERE CustomerID={0}", customer.CustomerId).ToListAsync();
            foreach (var order in ords)
            {
                await AttachProductsToOrder(order);
                await AttachOrderInfo(order);
            }
            return ords.ConvertAll(o => o.ConvertToModel());
        }

        public async Task<Product> GetProduct(Product product)
        {
            var prod = await _db.Products.FromSqlRaw("SELECT * FROM Store.Product WHERE ProductID = {0}", product.ProductId).FirstOrDefaultAsync();
            if (prod == null)
            {
                throw new ArgumentException("Invalid product");
            }
            return prod.ConvertToModel();
        }

        public async Task<List<Product>> GetProducts()
        {
            var prods = _db.Products.FromSqlRaw("SELECT * FROM Store.Product");
            var result = (from p in prods select p.ConvertToModel()).ToListAsync();
            return await result;
        }

        public async Task<List<Product>> GetProducts(Store store)
        {
            /*
            var prodQuery = _db.Products.FromSqlRaw("SELECT p.* FROM Store.Product p INNER JOIN Store.StoreProduct sp ON sp.ProductID = p.ProductID "
                + "WHERE sp.StoreID = {0}", store.StoreId);
            var prods = await (from p in prodQuery select p.ConvertToModel()).ToListAsync();
            */
            var prods = await GetStoreProducts(store.StoreId);
            return prods;
        }

        public async Task<List<Store>> GetStores()
        {
            var stores = await _db.Stores.FromSqlRaw("SELECT * FROM Store.Store").ToListAsync();
            return stores.ConvertAll(s => s.ConvertToModel());
        }

        public async Task<Store> GetStore(int storeId)
        {
            var store = await _db.Stores.FromSqlRaw("SELECT * FROM Store.Store WHERE StoreID={0}", storeId).FirstOrDefaultAsync();
            return store.ConvertToModel();
        }

        /*
        public void SetCustomer(Customer customer)
        {
            //_customerAdapter.Customer.Add(customer);
            _customerAdapter.Database.ExecuteSqlRaw("INSERT INTO Customer.Customer ([Name]) VALUES ({0})", customer.Name);
            _customerAdapter.SaveChanges();
        }
        */

        public async Task<Customer> AddCustomer(Customer customer)
        {
            //try
            //{
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO Customer.Customer (FirstName, LastName) VALUES ({0}, {1})", customer.FirstName, customer.LastName);
            await _db.SaveChangesAsync();
            var query = _db.Customers.FromSqlRaw("SELECT TOP(1) * FROM Customer.Customer WHERE FirstName={0} AND LastName={1} ORDER BY CustomerID DESC",
                        customer.FirstName, customer.LastName);
            try
            {
                var newCust = await query.FirstAsync();
                return newCust.ConvertToModel();
            }
            catch (InvalidOperationException)
            {
                // Error: newly created customer does not exist for some reason
                return null;
            }
            //}
        }

        public async Task<Product> AddProduct(Product product)
        {
            // Required: name, price
            // Optional: description, category
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO Store.Product (Name, Price, Description, CategoryID) VALUES ({0}, {1}, {2}, {3})",
                product.Name,
                product.Price,
                product.Description,
                product.CategoryID);
            await _db.SaveChangesAsync();
            // TODO: make this more better
            var prod = await _db.Products.FromSqlRaw("SELECT * FROM Store.Product WHERE Name = {0} AND Price = {1} and Description = {2}",
                product.Name,
                product.Price,
                product.Description
                ).FirstOrDefaultAsync();
            // TODO: better error handling/return type
            return prod.ConvertToModel();
        }

        public async Task<Store> AddStore(Store store)
        {
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO Store.Store (Name) VALUES ({0})", store.Name);
            await _db.SaveChangesAsync();
            // TODO: make this more better
            var s = await _db.Stores.FromSqlRaw("SELECT * FROM Store.Store WHERE Name = {0}", store.Name).FirstOrDefaultAsync();
            // TODO: better error handling/return type
            return s.ConvertToModel();
        }

        private async Task AttachInventoryQuantities(int storeId, Product product)
        {
            var storeProd = await _db.StoreProducts.FromSqlRaw("SELECT * FROM Store.StoreProduct WHERE StoreID = {0} AND ProductID = {1}",
                storeId,
                product.ProductId
            ).FirstAsync();
            _logger.LogInformation($"Attaching Quantity {storeProd.Quantity} to product {product.ProductId}");
            // TODO: I would prefer to figure out exactly why EF has an outdated copy of the info from the above query.
            // For whatever reason, Entity framework is not getting a fresh instance of StoreProduct entities here.
            //  This next line will force a reload from the database. This is fixing my issues.
            //  I found this solution in the discussion here:
            //    https://stackoverflow.com/questions/30524438/property-not-updated-after-savechanges-ef-database-first
            await _db.Entry(storeProd).ReloadAsync();
            _logger.LogInformation($"Attaching updated Quantity {storeProd.Quantity} to product {product.ProductId}");
            product.Quantity = storeProd.Quantity;
        }

        public async Task<List<Product>> GetStoreProducts(int storeId)
        {
            _logger.LogInformation($"Getting products for store {storeId}");
            var storeProductsQuery = _db.Products.FromSqlRaw("SELECT p.* FROM Store.Product p INNER JOIN Store.StoreProduct sp ON p.ProductID = sp.ProductID "
                + "WHERE sp.StoreID = {0}", storeId);
            var storeProducts = await (from p in storeProductsQuery select p.ConvertToModel()).ToListAsync();
            foreach (var sp in storeProducts)
            {
                await AttachInventoryQuantities(storeId, sp);
            }
            return storeProducts;
        }

        private async Task<(DBStore, DBProduct)> ValidateStoreAndProduct(Store store, Product product)
        {
            var dbStore = await _db.Stores.FromSqlRaw("SELECT * FROM Store.Store WHERE StoreID = {0}", store.StoreId).FirstOrDefaultAsync();
            if (dbStore == null)
            {
                throw new ArgumentException("Invalid store");
            }
            var dbProduct = await _db.Products.FromSqlRaw("SELECT * FROM Store.Product WHERE ProductID = {0}", product.ProductId).FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                throw new ArgumentException("Invalid product");
            }
            return (dbStore, dbProduct);
        }

        // TODO: this method is currently set to increment product quantity by one. this is very bad, quantity should have its own place
        public async Task<List<Product>> AddStoreProduct(Store store, Product product)
        {
            var (dbStore, dbProduct) = await ValidateStoreAndProduct(store, product);
            var storeProd = await _db.StoreProducts.FromSqlRaw("SELECT * FROM Store.StoreProduct WHERE StoreID = {0} AND ProductID = {1}",
                dbStore.StoreId,
                dbProduct.ProductId
            ).FirstOrDefaultAsync();
            if (storeProd == null)
            {
                await _db.Database.ExecuteSqlRawAsync("INSERT INTO Store.StoreProduct (StoreID, ProductID, Quantity) VALUES ({0}, {1}, 1)", dbStore.StoreId, dbProduct.ProductId);
            }
            else
            {
                await _db.Database.ExecuteSqlRawAsync("UPDATE Store.StoreProduct SET Quantity = {0} WHERE StoreID = {1} AND ProductID = {2}",
                    storeProd.Quantity + 1,
                    dbStore.StoreId,
                    dbProduct.ProductId
                );
            }
            await _db.SaveChangesAsync();
            return await GetStoreProducts(dbStore.StoreId);
        }

        public async Task<List<Product>> UpdateStoreQuantity(Store store, Product product, int quantityDelta)
        {
            var (dbStore, dbProduct) = await ValidateStoreAndProduct(store, product);
            var dbStoreProduct = await _db.StoreProducts.FromSqlRaw("SELECT * FROM Store.StoreProduct WHERE ProductID = {0} AND StoreID = {1}",
                dbProduct.ProductId,
                dbStore.StoreId
            ).FirstAsync();
            var newQuantity = dbStoreProduct.Quantity + quantityDelta;
            if (newQuantity > 0)
            {
                await _db.Database.ExecuteSqlRawAsync("UPDATE Store.StoreProduct SET Quantity = {0} WHERE StoreID = {1} AND ProductID = {2}",
                    newQuantity,
                    dbStore.StoreId,
                    dbProduct.ProductId
                );
                dbStoreProduct.Quantity = newQuantity;
                await _db.SaveChangesAsync();
            }
            else if (newQuantity == 0)
            {
                await DeleteStoreProduct(dbStore, dbProduct);
            }
            else
            {
                throw new ArgumentException("Invalid quantity -- new quantity must be non-negative");
            }
            return await GetStoreProducts(dbStore.StoreId);
        }

        private async Task DeleteStoreProduct(DBStore store, DBProduct product)
        {
            await _db.Database.ExecuteSqlRawAsync("DELETE FROM Store.StoreProduct WHERE ProductID = {0} AND StoreID = {1}", product.ProductId, store.StoreId);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Product>> RemoveStoreProduct(Store store, Product product)
        {
            var (dbStore, dbProduct) = await ValidateStoreAndProduct(store, product);
            await DeleteStoreProduct(dbStore, dbProduct);
            return await GetStoreProducts(dbStore.StoreId);
        }
    }

}
