using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [Controller]
    [Route("DrivingLicense")]
    public class DrivingLicenseController : Controller
    {
        IConfiguration _configuration;
        public DrivingLicenseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public JsonResult GetDrivingLicenses()
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // SELECT  dl.Driving_id,dl.User_id,dl.Driving_name,dl.Location,user=dlu from Driving_License dl join user u on dl.user_id = u.user_id
                dynamic response = (from dl in context.Driving_License
                                    join u in context.User on dl.User_id equals u.User_id into joinData
                                    from dlu in joinData.DefaultIfEmpty()
                                    select new
                                    {
                                        dl.Driving_id,
                                        dl.User_id,
                                        dl.Driving_name,
                                        dl.Location,
                                        user = dlu
                                    }).ToList();
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetDrivingLicenseById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // Select dl.Driving_id,dl.User_id, dl.Driving_name,dl.Location,u.* from Driving_License dl join user u on dl.user_id = u.user_id
                var data = (from dl in context.Driving_License
                            join u in context.User on dl.User_id equals u.User_id into joinData
                            from dlu in joinData.DefaultIfEmpty()
                            select new
                            {
                                dl.Driving_id,
                                dl.User_id,
                                dl.Driving_name,
                                dl.Location,
                                user = dlu
                            });
                result = new
                {
                    response = data.Where(e => e.Driving_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);
        }
        [HttpPost("[action]")]
        public JsonResult PostDrivingLicense([FromBody] Driving_License dlData)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    // insert into Driving_License values(?,?,?)
                    response = context.Driving_License.Add(dlData).ToString();
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
                    response
                };
            }
            return new JsonResult(result);
        }
        [HttpPut("[action]/{id}")]
        public JsonResult PutDrivingLicenseById([FromRoute] int id, [FromBody] Driving_License drivingLicenseData)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    // update Driving_License set ? = ? where ;
                    var drivingLicense = context.Driving_License.Where(e => e.Driving_id == id).FirstOrDefault();
                    if (drivingLicense == null) return new JsonResult(result);
                    drivingLicense.Driving_name = drivingLicenseData.Driving_name;
                    drivingLicense.User_id = drivingLicenseData.User_id;
                    drivingLicense.Location = drivingLicenseData.Location;


                    response = context.Driving_License.Update(drivingLicense).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteDrivingLicenseById([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var drivingLicense = context.Driving_License.Where(e => e.Driving_id == id).FirstOrDefault();
                    response = context.Driving_License.Remove(drivingLicense).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);

        }
    }


}
