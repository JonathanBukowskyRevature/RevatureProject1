using Microsoft.AspNetCore.Mvc;
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
    public class LoginController : ControllerBase
    {

        public class UserLoginInfo
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class LoginResponse
        {
            public bool success { get; set; }
            public Customer user { get; set; }
        }

        private StoreApp _app;
        private ILogger<LoginController> _log;
        public LoginController(StoreApp app, ILogger<LoginController> logger)
        {
            _app = app;
            _log = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<LoginResponse> LogIn(UserLoginInfo user)
        {
            string username = user.username;
            string password = user.password;
            _log.LogInformation("Received login request for username '{0}' and password '{1}'", username, password);
            // TODO: are action results appropriate here?
            var response = new LoginResponse() { success = false, user = null };
            if (username == null || username == "" || password == null || password == "")
            {
                return response;
            }
            var cust = await _app.LoginCustomer(username, password);
            if (cust == null)
            {
                return response;
            }
            response.success = true;
            response.user = cust;
            return response;
        }
    }
}
