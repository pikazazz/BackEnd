using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("[action]")]
        public JsonResult PostUser([FromBody] User userData)
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    // insert into User values(?,?,?)
                    response = context.User.Add(userData).ToString();
                    context.SaveChanges(); result = new
                    {
                        response
                    };

                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
                result = new
                {
                    response,
                    data = userData
                };
            }

            return new JsonResult(result);

        }
        [HttpGet("[action]")]
        public JsonResult GetUsers()
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    // select u.*, r.Role_name, r.Role_description, rfn.*, dt.*, wt.* from user u 
                    // join role r on u.Role_id = r.Role_id 
                    // join ReservationForNow rfn on u.User_id = rfn.User_id
                    // join Writing_Test wt join rfn.Res_id = wt.Res_id 
                    // join Driving_Test dt on rfn.Res_id = dt.Res_id
                    response = (from u in context.User
                                join r in context.Role on u.Role_id equals r.Role_id into joinData
                                from user_role in joinData.DefaultIfEmpty()
                                join rfn in context.ReservationForNow on u.User_id equals rfn.User_id into urfn_join
                                from urfn in urfn_join.DefaultIfEmpty()
                                join wt in context.Writing_Test on urfn.res_id equals wt.res_id into wt_join
                                from rwt in wt_join.DefaultIfEmpty()
                                join dt in context.Driving_Test on urfn.res_id equals dt.res_id into dt_join
                                from rdt in dt_join.DefaultIfEmpty()
                                select new { user = u, user_role.Role_name, user_role.Role_description, reservation_for_now = urfn, driving_test = rdt, writing_test = rwt }).ToList()

                };
            }
            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetUserById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // select u.*, r.Role_name, r.Role_description, rfn.*, dt.*, wt.* from user u 
                // join role r on u.Role_id = r.Role_id 
                // join ReservationForNow rfn on u.User_id = rfn.User_id
                // join Writing_Test wt join rfn.Res_id = wt.Res_id 
                // join Driving_Test dt on rfn.Res_id = dt.Res_id
                // where User_id = ?
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
                    response = joinUser.Where(e => e.user.User_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteUserById([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            var user = new User();
            using (var context = new DB(_configuration))
            {
                try
                {
                    user = context.User.Where(e => e.User_id == id).FirstOrDefault();
                    // delete User where user_id = ?
                    response = context.User.Remove(user).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = user
                };
            }

            return new JsonResult(result);

        }

        [HttpPut("[action]/{id}")]
        public JsonResult PutUserById([FromRoute] int id, [FromBody] User userData)
        {
            dynamic result = null;
            dynamic response = null;
            var user = new User();
            using (var context = new DB(_configuration))
            {
                try
                {
                    user = context.User.Where(e => e.User_id == id).FirstOrDefault();
                    if (user == null) return new JsonResult(result);
                    user.Firstname = userData.Firstname ?? user.Firstname;
                    user.Lastname = userData.Lastname ?? user.Lastname;
                    user.Password = userData.Password ?? user.Password;
                    user.Role_id = userData.Role_id ?? user.Role_id;
                    user.user_Address = userData.user_Address ?? user.user_Address;
                    user.CitizenId = userData.CitizenId ?? user.CitizenId;
                    user.dateOfBirth = userData.dateOfBirth ?? user.dateOfBirth;
                    user.Driving_id = userData.Driving_id ?? user.Driving_id;
                    user.user_Phone = userData.user_Phone ?? user.user_Phone;
                    user.Email = userData.Email ?? user.Email;
                    user.Status = userData.Status ?? user.Status;
                    // update User set ? = ? where User_id = ?;
                    response = context.User.Update(user).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = user
                };
            }

            return new JsonResult(result);

        }

    }
}
