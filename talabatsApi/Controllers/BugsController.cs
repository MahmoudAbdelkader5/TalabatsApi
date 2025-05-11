using Microsoft.AspNetCore.Mvc;
using talabatRepository.Data;
using talabatsApi.Error;

namespace talabatsApi.Controllers
{
    public class BugsController : ApiBaseController
    {
        private readonly TalabatDbContext context;

        public BugsController(TalabatDbContext context)
        {
            this.context = context;
        }
        [HttpGet("Notfound")]
        public ActionResult getNotFound()
        {
            var result = context.Products.Find(100);
            if (result == null) return NotFound(new ApiResponse(404));
            return Ok(result);
        }
        [HttpGet("badRequest/{id}")]
        public ActionResult badRequest (int id)
        {
            var result = context.Products.Find(100);
            if (result == null) return NotFound(new ApiValidationError());
            return Ok(result);

          
        }


    }
}
