using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using core7_cassandra_angular14.Controllers.Model;
using core7_cassandra_angular14.Entities;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace core7_cassandra_angular14.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class Signin : ControllerBase {

        private IAuthService _authService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Signin> _logger;
        public Signin(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IAuthService authService,
            ILogger<Signin> logger
            )
        {   
            _configuration = configuration;  
            _authService = authService;
            _logger = logger;
            _env = env;
        }  

        [HttpPost("/signin")]
        public IActionResult signinUser([FromBody]SigninModel model) {
          try {
            User user = _authService.SignUser(model.Username);
            if (user is not null) {
                    if (!BCrypt.Net.BCrypt.Verify(model.Pasword, user.Pasword)) {
                        return Ok(new {statuscode=404, message="Incorrect Password..."});
                    }
                    if (user.Isactivated == 0) {
                        return NotFound(new {statuscode=404, message="Please activate your account, check your email inbox."});
                    }
                    if (user.Isblocked == 1) {
                        return NotFound(new {statuscode=400, message="Your account has been blocked, please contact Web Master."});
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var xkey = _configuration["AppSettings:Secret"];
                    var key = Encoding.ASCII.GetBytes(xkey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    return Ok(new { 
                        statuscode = 200,
                        message = "Login Successfull, please wait..",
                        id = user.Id,
                        lastname = user.Lastname,
                        firstname = user.Firstname,
                        username = user.Username,
                        roles = user.Role,
                        isactivated = user.Isactivated,
                        isblocked = user.Isblocked,
                        profilepic = user.Profilepic,
                        qrcodeurl = user.Qrcodeurl,
                        token = tokenString
                        });
            }
            return Ok(new {statuscode=404 ,message="Username not found, please register first..."});

          } catch(Exception) {
            return Ok(new {statuscode = 404, message= "Username not found, please register first..."});
          }
        }
    }
}