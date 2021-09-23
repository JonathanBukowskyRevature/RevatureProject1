using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Project1.StoreApplication.Business;
using Project1.StoreApplication.Models;
using Project1.StoreApplication.Storage;
using Project1.StoreApplication.Test.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project1.StoreApplication.Test.Business
{
    public class StoreAppTest
    {
        private static IStorage _stor = new DBStorageMock(true);
        private static IStorage _fail_stor = new DBStorageMock(false);

        private static List<Product> _testProds = new List<Product>()
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
        };

        public static List<Customer> _testCusts = new List<Customer>()
        {
            new Customer()
            {
                FirstName = "Test fn 1",
                LastName = "Test ln 1",
                CustomerId = 1
            },
            new Customer()
            {
                FirstName = "Test fn 2",
                LastName = "Test ln 2",
                CustomerId = 2
            },
            new Customer()
            {
                FirstName = "Test fn 6",
                LastName = "Test ln 6",
                CustomerId = 6
            },
        };

        public static List<Store> _testStores = new List<Store>()
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
        };

        public static IEnumerable<object[]> GetFailDB()
        {
            ILogger<StoreApp> log = new NullLogger<StoreApp>();
            ICarts cartMan = new CartManMock();
            StoreApp app = new StoreApp(_fail_stor, cartMan, log);
            foreach (var s in _testStores)
            {
                foreach (var c in _testCusts)
                {
                    yield return new object[] { app, c, s };
                }
            }
        }

        public static IEnumerable<object[]> GetDB()
        {
            ILogger<StoreApp> log = new NullLogger<StoreApp>();
            ICarts cartMan = new CartManMock();
            StoreApp app = new StoreApp(_stor, cartMan, log);
            foreach (var s in _testStores)
            {
                foreach (var c in _testCusts)
                {
                    yield return new object[] { app, c, s };
                }
            }
            yield return new object[] { app, true };
        }

        [Theory]
        [MemberData(nameof(GetFailDB))]
        [MemberData(nameof(GetDB))]
        public async Task Test_StoreApp_GetCart(StoreApp sut, Customer c, Store s)
        {
            var fail = !(c.CustomerId >= 5 || s.StoreId >= 5);
            var cart = await sut.GetCart(c, s);
            if (fail)
            {
                // cart should be empty
                Assert.Empty(cart);
            }
            else
            {
                Assert.Equal(3, cart.Count);
                var (p1, q1) = cart[0];
                Assert.Contains("1", p1.Name);
                Assert.Contains("1", p1.Description);
                Assert.Equal(1, p1.ProductId);
                Assert.Equal(1, q1);
            }
        }

        public static IEnumerable<object[]> GetAddToCartDataInvalid()
        {
            foreach (var p in _testProds)
            {
                foreach (var s in _testStores)
                {
                    foreach (var c in _testCusts)
                    {
                        yield return new object[] { p, s, c, 4 };
                    }
                }
            }
        }
        public static IEnumerable<object[]> GetAddToCartData()
        {
            foreach (var p in _testProds)
            {
                var goodP = new Product()
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    Quantity = 10
                };
                foreach (var s in _testStores)
                {
                    foreach (var c in _testCusts)
                    {
                        yield return new object[] { goodP, s, c, 3 };
                    }
                }
            }
        }


        [Theory]
        [MemberData(nameof(GetAddToCartDataInvalid))]
        public void Test_StoreApp_AddProduct_InvalidQuantity(Product p, Store s, Customer c, int q)
        {
            ILogger<StoreApp> log = new NullLogger<StoreApp>();
            ICarts cartMan = new CartManMock();
            var sut = new StoreApp(_stor, cartMan, log);
            Assert.Throws(Type.GetType(nameof(ArgumentException)), async () =>
            {
                await sut.AddProductToCart(p, c, s, q);
            });
        }

        [Theory]
        [MemberData(nameof(GetAddToCartData))]
        public async Task Test_StoreApp_AddProduct(Product p, Store s, Customer c, int q)
        {
            ILogger<StoreApp> log = new NullLogger<StoreApp>();
            ICarts cartMan = new CartManMock();
            var sut = new StoreApp(_stor, cartMan, log);

            await sut.AddProductToCart(p, c, s, q);
        }

        public static IEnumerable<object[]> GetValidLogins()
        {
            yield return new object[] { new Customer() };
        }
    }
}
