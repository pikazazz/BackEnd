using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [Controller]
    [Route("DrivingTest")]
    public class DrivingTestController : Controller
    {
        IConfiguration _configuration;
        public DrivingTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public JsonResult GetDrivingTestList()
        {
            dynamic? result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    // select dt.drivingTest_id,dt.drivingTest_score, s.*, rfn.* from Driving_Test dt join Staff s on dt.staff_id = s.Staff_id join ReservationForNow rfn on rfn.res_id = dt.res_id
                    response = (from dt in context.Driving_Test join s in context.Staff on dt.staff_id equals s.Staff_id into joinData from dts in joinData.DefaultIfEmpty() join rfn in context.ReservationForNow on dt.res_id equals rfn.res_id into joinData2 from dtrfn in joinData2.DefaultIfEmpty() select new { dt.drivingTest_id, dt.drivingTest_score, staff = dts, reservationForNow = dtrfn }).ToList()
                };
            }
            return new JsonResult(result);
        }

        [HttpPost("[action]")]
        public JsonResult PostDrivingTest([FromBody] Driving_Test drivingTestData)
        {
            dynamic? result = null;
            using (var context = new DB(_configuration))
            {
                dynamic? response = null;
                try
                {
                    // insert into Driving_Test values(?,?,?)
                    response = context.Driving_Test.Add(drivingTestData).ToString();
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
                result = new
                {
                    response,
                    data=drivingTestData
                };
            }
            return new JsonResult(result);
        }

        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteDrivingTestById([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var drivingTest = context.Driving_Test.Where(e => e.drivingTest_id == id).FirstOrDefault();
                    // delete Driving_Test where drivingTest_id = ?
                    response = context.Driving_Test.Remove(drivingTest).ToString();
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
        public JsonResult PutWritingTestById([FromRoute] int id, [FromBody] Driving_Test drivingTestData)
        {
            dynamic? result = null;
            dynamic? response = null;
            var drivingTest = new Driving_Test();
            using (var context = new DB(_configuration))
            {
                try
                {
                    drivingTest = context.Driving_Test.Where(e => e.drivingTest_id == id).FirstOrDefault();
                    if (drivingTest == null) return new JsonResult(result);
                    drivingTest.drivingTest_score = drivingTestData.drivingTest_score ?? drivingTest.drivingTest_score;
                    drivingTest.staff_id = drivingTestData.staff_id ?? drivingTest.staff_id;
                    drivingTest.res_id = drivingTestData.res_id ?? drivingTest.res_id;

                    // update Driving_Test set ? = ? where drivingTest_id = ?;
                    response = context.Driving_Test.Update(drivingTest).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data = drivingTest
                };
            }

            return new JsonResult(result);

        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetDrivingTestById([FromRoute] int id)
        {
            dynamic? result = null;
            using (var context = new DB(_configuration))
            {

                var data = (from dt in context.Driving_Test join s in context.Staff on dt.staff_id equals s.Staff_id into joinData from dts in joinData.DefaultIfEmpty() join rfn in context.ReservationForNow on dt.res_id equals rfn.res_id into joinData2 from dtrfn in joinData2.DefaultIfEmpty() select new { dt.drivingTest_id, dt.drivingTest_score, staff = dts, reservationForNow = dtrfn });
                result = new
                {
                    response = data.Where(e => e.drivingTest_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
    }
}
