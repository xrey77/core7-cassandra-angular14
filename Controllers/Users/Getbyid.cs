using core7_cassandra_angular14.Entities;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class Getbyid : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Getbyid> _logger;
        public Getbyid(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<Getbyid> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpGet("/api/getuserbyid/{id}")]
        public IActionResult getbyUserid(int id){
            try {
                User user = _userService.GetById(id);
                return Ok(new {
                    statuscode = 200,
                    message = "User found, please wait.",
                    user = user
                });

            } catch(Exception ex) {
                return NotFound(new {
                    statuscode = 404,
                    message = ex.Message
                });

            }

        }

    }
    
}