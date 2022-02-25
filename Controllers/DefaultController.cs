using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("")]

    public class DefaultController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DefaultController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public JsonResult Login([FromBody] User? userData)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // SELECT  u.Firstname, u.Lastname, u.user_Address, u.Password, u.dateOfBirth, u.CitizenId, user_role.Role_name, user_role.Role_description from user u join role r on u.role_id = r.role_id where u.CitizenId = ? and Password = ?;
                var joinUser = (from u in context.User join r in context.Role on u.Role_id equals r.Role_id into joinData from user_role in joinData.DefaultIfEmpty() select new { u.Firstname, u.Lastname, u.user_Address, u.Password, u.dateOfBirth, u.CitizenId, user_role.Role_name, user_role.Role_description });
                result = new
                {
                    response = joinUser.Where(e => e.CitizenId == userData.CitizenId && e.Password == userData.Password).FirstOrDefault(),
                };
            }
            return new JsonResult(result);
        }

    }


}
