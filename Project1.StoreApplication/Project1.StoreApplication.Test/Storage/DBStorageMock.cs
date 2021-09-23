using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Test.Storage
{
    public class DBStorageMock : IStorage
    {
        private bool _succeed;
        public DBStorageMock(bool succeed)
        {
            _succeed = succeed;
        }
        public Task<Customer> AddCustomer(Customer customer)
        {
            if (_succeed)
            {
                customer.CustomerId = 1;
                return Task.FromResult(customer);
            }
            else
            {
                return Task.FromResult<Customer>(null);
            }
        }

        public Task AddLogin(Customer customer, string username, string password)
        {
            // success
            if (!_succeed)
            {
                throw new ArgumentException();
            }
            return Task.CompletedTask;
        }

        public Task<Product> AddProduct(Product product)
        {
            if (!_succeed)
            {
                return Task.FromResult<Product>(null);
            }
            product.ProductId = 1;
            return Task.FromResult(product);
        }

        public Task<Store> AddStore(Store store)
        {
            if (!_succeed)
            {
                return Task.FromResult<Store>(null);
            }
            store.StoreId = 1;
            return Task.FromResult(store);
        }

        public Task<List<Product>> AddStoreProduct(Store store, Product product)
        {
            if (!_succeed)
            {
                return Task.FromResult<List<Product>>(null);
            }
            return Task.FromResult(new List<Product>()
            {
                new Product() {
                    ProductId = 1,
                    Name = "Product 1",
                    Description = "prod 1",
                    Price = 1.99m,
                    Quantity = 1
                },
                new Product() {
                    ProductId = 2,
                    Name = "Product 2",
                    Description = "prod 2",
                    Price = 2.99m,
                    Quantity = 2
                },
                new Product()
                {
                    ProductId = 3,
                    Name = "Product 3",
                    Description = "prod 3",
                    Price = 3.99m,
                    Quantity = 3
                }
            });
        }

        public Task<Order> CreateOrder(Customer customer, Store store, List<(Product, int)> products)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomer(Customer customer)
        {
            return Task.FromResult(customer);
        }

        public Task<List<Customer>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrders(Store store)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrders(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(Product product)
        {
            return Task.FromResult(product);
        }

        public Task<List<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProducts(Store store)
        {
            return Task.FromResult<List<Product>>(new()
            {
                new Product()
                {
                    ProductId = 1,
                    Name = "Product 1",
                    Description = "prod 1",
                    Price = 1.99m,
                    Quantity = 1
                },
                new Product()
                {
                    ProductId = 2,
                    Name = "Product 2",
                    Description = "prod 2",
                    Price = 2.99m,
                    Quantity = 2
                },
                new Product()
                {
                    ProductId = 3,
                    Name = "Product 3",
                    Description = "prod 3",
                    Price = 3.99m,
                    Quantity = 3
                }
            });
        }

        public async Task<Store> GetStore(int storeId)
        {
            var stores = await GetStores();
            return (stores.Find((s) => s.StoreId == storeId));
        }

        public Task<List<Store>> GetStores()
        {
            return Task.FromResult(new List<Store>()
            {
                new Store()
                {
                    Name = "Test store 1",
                    StoreId = 1
                },
                new Store()
                {
                    Name = "Test store 2",
                    StoreId = 2
                },
                new Store()
                {
                    Name = "Test store 6",
                    StoreId = 6
                },
            });
        }

        public Task<List<Product>> RemoveStoreProduct(Store store, Product product)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> UpdateStoreQuantity(Store store, Product product, int quantityDelta)
        {
            throw new NotImplementedException();
        }
    }
}
