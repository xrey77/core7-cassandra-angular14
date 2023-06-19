using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]    
    public class Delete : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Delete> _logger;
        public Delete(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<Delete> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpDelete("/api/deleteuser/{id}")]
        public IActionResult deleteUser(int id) {
            try
            {
               _userService.Delete(id);
            return Ok(new {statucode = 200, message = "User Id " + id.ToString() + " has been deleted."});
           }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
    
}