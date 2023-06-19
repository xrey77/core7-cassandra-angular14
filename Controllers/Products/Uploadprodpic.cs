using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace core7_cassandra_angular14.Controllers.Products
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiExplorerSettings(GroupName = "Upload Product Image")]
    [ApiController]
    [Route("[controller]")]
    public class Uploadprodpic : ControllerBase {

        private Cluster _cluster;
        private IMapper _mapper;
        private Cassandra.ISession _session;

        public Uploadprodpic() {
            _cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            // .WithCredentials("rey","rey")
            .WithPort(9042).Build();
            _session = _cluster.Connect("core7");
            _mapper = new Mapper(_session);
        }
        [HttpPost("/uplooadproductpic")]
        public IActionResult UploadPicture() {
            return Ok();
        }
    }
}