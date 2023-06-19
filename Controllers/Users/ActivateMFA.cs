using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Services;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]    
    public class ActivateMFA : ControllerBase {
        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ActivateMFA> _logger;
        public ActivateMFA(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<ActivateMFA> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpPut("/api/enablemfa/{id}")]
        public IActionResult EnableMFA(int id,MfaModel model) {
            if (model.Twofactorenabled == true) {
                var user = _userService.GetById(id);
                if(user != null) {
                    QRCode qrimageurl = new QRCode();
                    var fullname = user.Firstname + " " + user.Lastname;
                    TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
                    var setupInfo = twoFactor.GenerateSetupCode(fullname, user.Email, user.Secretkey, false, 3);
                    var imageUrl = setupInfo.QrCodeSetupImageUrl;
                    _userService.ActivateMfa(id, true, imageUrl);
                    return Ok(new {statuscode = 200, message="2-Factor Authenticator has been enabled."});
                } else {
                    return Ok(new {statuscode = 404, message="User not found."});
                }

            } else {
                _userService.ActivateMfa(id, false, null);
                return Ok(new {statuscode = 200, message="2-Factor Authenticator has been disabled."});
            }
        }

    }
}