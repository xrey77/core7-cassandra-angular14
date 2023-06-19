using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiExplorerSettings(GroupName = "List All Products")]
    [ApiController]
    [Route("[controller]")]    
    public class ListProducts : ControllerBase {
        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ListProducts> _logger;
        public ListProducts(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<ListProducts> logger
            )
        {   
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;
        }  
        [HttpGet("/listproducts/{page}")]
        public IActionResult listAllProducts(int page) {
            try {                
                int totalpage = _productService.TotPage();
                var prods = _productService.ListAll(page);
                return Ok(new {page = page, totpage = totalpage, products = prods});
            } catch(Exception ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }

    }    
}