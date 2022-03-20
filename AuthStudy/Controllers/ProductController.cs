using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthStudy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ProductController : ControllerBase
    {
        [HttpGet("AdminFunction")]
        [Authorize(Roles= "Admin")]
        public string AdminFunction() {
            return "AdminFunction 专属";
        }
        [HttpGet("UserFunction")]
        [Authorize(Roles = "User")]
        public string UserFunction()
        {
            return "UserFunction 专属";
        }
        [HttpGet("LaoBanFunction")]
        public string LaoBanFunction()
        {
            return "LaoBanFunction 专属";
        }
    }
}
