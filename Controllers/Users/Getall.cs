using core7_cassandra_angular14.Entities;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class Getall : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Getall> _logger;
        public Getall(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<Getall> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpGet("/api/getallusers")]
        public IActionResult getallUsers() {
            try {                
                var user = _userService.GetAll();
                return Ok(user);
            } catch(Exception ex) {
               return Ok(new {statuscode = 404, Message = ex.Message});
            }
        }
    }
    
}