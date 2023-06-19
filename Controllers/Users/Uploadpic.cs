using core7_cassandra_angular14.Models;
using core7_cassandra_angular14.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Users 
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiExplorerSettings(GroupName = "Upload User Image")]
    [ApiController]
    [Route("[controller]")]   
    [Authorize] 
    public class Uploadpic : ControllerBase {
        private IUserService _userService;
        private readonly IConfiguration _configuration;  
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Uploadpic> _logger;
        public Uploadpic(
            IConfiguration configuration,            
            IWebHostEnvironment env,
            IUserService userService,
            ILogger<Uploadpic> logger
            )
        {   
            _configuration = configuration;  
            _userService = userService;
            _logger = logger;
            _env = env;
        }  

        [HttpPost("/api/uploadpicture")]
        public IActionResult uploadPicture([FromForm]UploadpicModel model) {
                if (model.Profilepic.FileName != null)
                {
                    try
                    {
                        string ext= Path.GetExtension(model.Profilepic.FileName);

                        var folderName = Path.Combine("Resources", "users/");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        var newFilename =pathToSave + "00" + model.Id + ".jpg";

                        using var image = SixLabors.ImageSharp.Image.Load(model.Profilepic.OpenReadStream());
                        image.Mutate(x => x.Resize(100, 100));
                        image.Save(newFilename);

                        if (model.Profilepic != null)
                        {
                            string file = "http://localhost:5168/resources/users/00"+model.Id.ToString()+".jpg";
                            _userService.UpdatePicture(model.Id, file);                            
                        }
                        return Ok(new { statuscode = 200, message = "Profile Picture has been update."});
                        
                    }
                    catch (Exception ex)
                    {
                        return Ok(new {statuscode = 200, message =ex.Message});
                    }

                }
                return Ok(new { statuscode = 404, message = "Profile Picture not found."});

        }

    }
}