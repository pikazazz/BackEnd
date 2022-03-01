using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("Role")]
    public class RoleController : Controller
    {
        IConfiguration _configuration;
        public RoleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("[action]")]
        public JsonResult GetRoles()
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                result = new
                {
                    // select * from reole
                    response = context.Role.ToList()
                };
            }

            return new JsonResult(result);

        }

    }
}
