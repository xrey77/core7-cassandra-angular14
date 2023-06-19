using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiExplorerSettings(GroupName = "Update Product")]
    [ApiController]
    [Route("[controller]")]
    public class UpdateProduct : ControllerBase {
        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<UpdateProduct> _logger;
        public UpdateProduct(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<UpdateProduct> logger
            )
        {   
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;
        }  
        [HttpGet("/updateproduct")]
        public IActionResult updateProduct() {
            return Ok();
        }
    }
}