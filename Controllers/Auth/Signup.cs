using Microsoft.AspNetCore.Mvc;
using core7_cassandra_angular14.Services;
using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace core7_cassandra_angular14.Controllers.Auth
{    
    [ApiExplorerSettings(GroupName = "Sign-up or Account Registration")]
    [ApiController]
    [Route("[controller]")]
    public class Signup : ControllerBase {

        private IAuthService _authService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Signup> _logger;
        
         IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

        public Signup(
            IWebHostEnvironment env,
            IAuthService authService,
            ILogger<Signup> logger
            )
        {   
            _authService = authService;
            _logger = logger;
            _env = env;
        }  

        [HttpPost("/signup")]
        public IActionResult signupUser([FromBody]SignupModel model){
            try {

                var tokenHandler = new JwtSecurityTokenHandler();
                var xkey = config["AppSettings:Secret"];
                var key = Encoding.ASCII.GetBytes(xkey);

                // CREATE SECRET KEY FOR USER TOKEN===============
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, model.Email)
                    }),
                    // Expires = DateTime.UtcNow.AddDays(7),
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var secret = tokenHandler.CreateToken(tokenDescriptor);
                var secretkey = tokenHandler.WriteToken(secret);
                
                User user = new User();
                user.Firstname = model.Firstname;
                user.Lastname = model.Lastname;
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.Role = "USER";
                user.Isactivated =0;
                user.Isblocked = 0;
                user.Mailtoken = 0;
                user.Username = model.Username;
                user.Secretkey = secretkey.ToUpper();  
                user.Profilepic = "http://localhost:5168/resources/users/pix.png";
                if (model.Password is not null) {
                    user.Pasword = BCrypt.Net.BCrypt.HashPassword(model.Password);;
                }
                var x1 = _authService.SignUp(user);
                return Ok(new {statuscode = 200, message = "You have registered successfully."});
            }
            catch (Exception ex)
            {
                return Ok(new { statuscode = 404, message = ex.Message });
            }

        }
    }
    
}