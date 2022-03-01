using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("Staff")]
    public class StaffController : Controller
    {
        IConfiguration _configuration;
        public StaffController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("[action]")]
        public JsonResult GetStaffs()
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                result = new
                {
                    // select * from staff;
                    response = context.Staff.ToList()
                };
            }

            return new JsonResult(result);

        }

        [HttpPost("[action]")]
        public JsonResult PostStaff([FromBody] Staff staffData)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                      // insert into Staff values(?,?,?)
                    response = context.Staff.Add(staffData).ToString();
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
                    data=staffData
                };
            }
            return new JsonResult(result);
        }

        [HttpGet("[action]/{id}")]
        public JsonResult GetStaffById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    response = context.Staff.Where(e => e.Staff_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteStaffById([FromRoute] int id)
        {
            dynamic result = null;
            dynamic response = null;
            var staff = new Staff();
            using (var context = new DB(_configuration))
            {
                try
                {
                    staff = context.Staff.Where(e => e.Staff_id == id).FirstOrDefault();
                    // delete Staff where Staff_id = ?
                    response = context.Staff.Remove(staff).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = staff
                };
            }

            return new JsonResult(result);

        }
        [HttpPut("[action]/{id}")]
        public JsonResult PutStaffById([FromRoute] int id, [FromBody] Staff staffData)
        {
            dynamic? result = null;
            dynamic? response = null;
            var staff = new Staff();
            using (var context = new DB(_configuration))
            {
                try
                {
                    staff = context.Staff.Where(e => e.Staff_id == id).FirstOrDefault();
                    if (staff == null) return new JsonResult(result);
                    staff.Staff_name = staffData.Staff_name ?? staff.Staff_name;
                    staff.Staff_lastname = staffData.Staff_lastname ?? staff.Staff_lastname;
                    staff.Staff_phone = staffData.Staff_phone ?? staff.Staff_phone;

                     // update Staff set ? = ? where Staff_id = ?;
                    response = context.Staff.Update(staff).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = staff
                };
            }

            return new JsonResult(result);

        }
    }
}
