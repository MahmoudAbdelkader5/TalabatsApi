using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabatsApi.Error;

namespace talabatsApi.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class errorController : ControllerBase
    {
        public IActionResult HandleError(int code)
        {
            return new ObjectResult(new ApiResponse(code));
            
        }
    }
}
