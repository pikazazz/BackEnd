using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [ApiController]
    [Route("User")]
    public class RenewLicenseController : Controller
    {
        IConfiguration _configuration;
        public RenewLicenseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("[action]")]
        public JsonResult PostRenewLicense([FromBody] RenewLicense renewLicenseData)
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    // insert into RenewLicense values(?,?,?)
                    response = context.RenewLicense.Add(renewLicenseData).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
                result = new
                {
                    response,
                    data = renewLicenseData
                };
            }

            return new JsonResult(result);

        }
        [HttpGet("[action]")]
        public JsonResult GetRenewLicenseList()
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    // select rl.user_id,rl.name,rl.email,rl.phone,rl.citizenId,rl.drivingId,rl.dateOfBirth,d.* from  RenewLicense rl join driving_license on rl.Driving_id = rl.drivingId
                    response = (from rl in context.RenewLicense
                                join d in context.Driving_License on rl.drivingId equals d.Driving_id into uldJoin
                                from uldData in uldJoin.DefaultIfEmpty()
                                select new { rl.user_id, rl.name, rl.email, rl.phone, rl.citizenId, rl.drivingId, rl.dateOfBirth, driving = uldData }).ToList()

                };
            }
            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetRenewLicenseById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // select rl.user_id,rl.name,rl.email,rl.phone,rl.citizenId,rl.drivingId,rl.dateOfBirth,d.* from  RenewLicense rl join driving_license on rl.Driving_id = rl.drivingId where user_id = ?
                var joinUser = (from rl in context.RenewLicense
                                join d in context.Driving_License on rl.drivingId equals d.Driving_id into uldJoin
                                from uldData in uldJoin.DefaultIfEmpty()
                                select new { rl.user_id, rl.name, rl.email, rl.phone, rl.citizenId, rl.drivingId, rl.dateOfBirth, driving = uldData });


                result = new
                {
                    response = joinUser.Where(e => e.user_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteRenewLicenseById([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var user = context.RenewLicense.Where(e => e.user_id == id).FirstOrDefault();
                    // delete RenewLicense where user_id = ?
                    response = context.RenewLicense.Remove(user).ToString();
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

        [HttpPut("[action]/{id}")]
        public JsonResult PutRenewLicenseById([FromRoute] int id, [FromBody] RenewLicense renewLicenseData)
        {
            dynamic result = null;
            dynamic response = null;
            var renewLicense = new RenewLicense();
            using (var context = new DB(_configuration))
            {
                try
                {
                    renewLicense = context.RenewLicense.Where(e => e.user_id == id).FirstOrDefault();
                    if (renewLicense == null) return new JsonResult(result);
                    renewLicense.name = renewLicenseData.name ?? renewLicense.name;
                    renewLicense.email = renewLicenseData.email ?? renewLicense.email;
                    renewLicense.phone = renewLicenseData.phone ?? renewLicense.phone;
                    renewLicense.citizenId = renewLicenseData.citizenId ?? renewLicense.citizenId;
                    renewLicense.drivingId = renewLicenseData.drivingId ?? renewLicense.drivingId;
                    renewLicense.dateOfBirth = renewLicenseData.dateOfBirth ?? renewLicense.dateOfBirth;
                    // update RenewLicense set ? = ? where user_id = ?;
                    response = context.RenewLicense.Update(renewLicense).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = renewLicense
                };
            }

            return new JsonResult(result);

        }

    }
}
