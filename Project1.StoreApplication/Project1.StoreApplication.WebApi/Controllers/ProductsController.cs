using Microsoft.AspNetCore.Mvc;
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
    public class ProductsController : ControllerBase
    {
        private StoreApp _app;
        public ProductsController(StoreApp app)
        {
            _app = app;
        }

        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            return await _app.GetProducts();
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<Product> AddProduct(Product product)
        {
            // TODO: add categories
            product.CategoryID = null;
            return await _app.AddProduct(product);
        }
    }
}
