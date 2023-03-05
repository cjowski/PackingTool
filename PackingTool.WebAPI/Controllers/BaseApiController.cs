using Microsoft.AspNetCore.Mvc;

namespace PackingTool.WebAPI.Controllers
{
    [ApiController]
    [Attributes.UserTokenAuthorize]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase { }
}
