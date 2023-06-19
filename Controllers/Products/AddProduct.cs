using core7_cassandra_angular14.Entities;
using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiExplorerSettings(GroupName = "Add Product")]
    [ApiController]
    [Route("[controller]")]
    public class AddProduct : ControllerBase {

        private IProductService _productService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AddProduct> _logger;
        public AddProduct(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IProductService productService,
            ILogger<AddProduct> logger
            )
        {   
            _configuration = configuration;  
            _productService = productService;
            _logger = logger;
            _env = env;
        }  
        
        [HttpPost("/addproduct")]
        public IActionResult addProduct([FromBody]ProductModel model) {
            try {
                Product prod = new Product();
                prod.Descriptions = model.Descriptions;
                prod.Qty = model.Qty;
                prod.Unit = model.Unit;
                prod.Cost_price = model.Cost_price;
                prod.Sell_price = model.Sell_price;
                prod.Sale_price = model.Sale_price;
                prod.Alert_level = model.Alert_level;
                prod.Category = model.Category;
                prod.Critical_level = model.Critical_level;
                _productService.AddProduct(prod);
                return Ok(new {statuscode = 200, message="New product added."});

            }
            catch(Exception ex) {
                return NotFound(new {statuscode = 400, message = ex.Message});
            }
        }
    }

}