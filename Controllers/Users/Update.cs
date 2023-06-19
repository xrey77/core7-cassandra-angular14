using core7_cassandra_angular14.Entities;
using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users
{
    public class Update : ControllerBase {

        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Update> _logger;
        public Update(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<Update> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpPost("/api/updateprofile/{id}")]
        public IActionResult updateUser(int id, [FromBody]UserUpdate model) {
            User user = new User();
            user.Id = id;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Mobile = model.Mobile;
            user.Pasword = model.Pasword;
            try
            {
                _userService.Update(user);
                return Ok(new {statuscode=200, message="Your profile has been updated."});
            }
            catch (Exception ex)
            {
                return BadRequest(new { statuscode = 404, message = ex.Message });
            }
        }

    }
    
}