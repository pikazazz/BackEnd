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
                // select u.*, r.Role_name, r.Role_description, rfn.*, dt.*, wt.* from user u 
                // join role r on u.Role_id = r.Role_id 
                // join ReservationForNow rfn on u.User_id = rfn.User_id
                // join Writing_Test wt join rfn.Res_id = wt.Res_id 
                // join Driving_Test dt on rfn.Res_id = dt.Res_id
                var joinUser = (from u in context.User
                                join r in context.Role on u.Role_id equals r.Role_id into joinData
                                from user_role in joinData.DefaultIfEmpty()
                                join rfn in context.ReservationForNow on u.User_id equals rfn.User_id into urfn_join
                                from urfn in urfn_join.DefaultIfEmpty()
                                join wt in context.Writing_Test on urfn.res_id equals wt.res_id into wt_join
                                from rwt in wt_join.DefaultIfEmpty()
                                join dt in context.Driving_Test on urfn.res_id equals dt.res_id into dt_join
                                from rdt in dt_join.DefaultIfEmpty()
                                select new { user = u, user_role.Role_name, user_role.Role_description, reservation_for_now = urfn, driving_test = rdt, writing_test = rwt });
                result = new
                {
                    response = joinUser.Where(e => e.user.Email == userData.Email && e.user.Password == userData.Password).FirstOrDefault(),
                    sql = joinUser.ToString()
                };
            }
            return new JsonResult(result);
        }

    }


}
