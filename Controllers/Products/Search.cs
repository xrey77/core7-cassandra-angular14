using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    [ApiController]
    [Route("[controller]")]    
    public class Search : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Search> _logger;
        public Search(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<Search> logger
            )
        {   
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;
        }  

        [HttpPost("/searchproducts")]
        public IActionResult SearchProducts(ProductSearch prod) {
            try {                
                var products = _productService.SearchAll(prod.Search);
                if (products is not null) {
                    return Ok(new {products=products});
                } else {
                    return Ok(new {statuscode=404, message="No Data found."});
                }
            } catch(Exception ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }    }
    
}