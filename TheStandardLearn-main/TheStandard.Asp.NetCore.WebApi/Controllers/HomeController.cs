using Microsoft.AspNetCore.Mvc;

namespace TheStandard.Asp.NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetHome() =>
            "Thank You Mario! But the princess is in another castle.";
    }
}
