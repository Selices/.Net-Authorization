using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthStudy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("GetInfo")]
        public string GetInfo() {
            return "这是测试控制器";
        }
    }
}
