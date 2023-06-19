using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    [ApiController]
    [Route("[controller]")]
    public class Update : ControllerBase {
        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Update> _logger;
        public Update(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<Update> logger
            )
        {   
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;
        }  
        public IActionResult updateProduct() {
            return Ok();
        }
    }
}