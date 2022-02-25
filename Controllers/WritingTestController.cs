using Microsoft.AspNetCore.Mvc;

namespace driver_app_api
{
    [Controller]
    [Route("WritingTest")]
    public class WritingTestController : Controller
    {
        IConfiguration _configuration;
        public WritingTestController(IConfiguration configuration) { _configuration = configuration; }
        [HttpGet("[action]")]
        public JsonResult GetWritingTestList()
        {
            dynamic? result = null;
            using (var context = new DB(_configuration))
            {
                result = new
                {
                    response = (from wt in context.Writing_Test join s in context.Staff on wt.staff_id equals s.Staff_id into joinData from wts in joinData.DefaultIfEmpty() join rfn in context.ReservationForNow on wt.res_id equals rfn.res_id into joinData2 from wtrfn in joinData2.DefaultIfEmpty() select new { wt.writingTest_id, wt.writingTest_score, staff = wts, reservationForNow = wtrfn }).ToList()
                };
            }
            return new JsonResult(result);
        }
        [HttpPost("[action]")]
        public JsonResult PostWritingTest([FromBody]Writing_Test writingTestData)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    response = context.Writing_Test.Add(writingTestData).ToString();
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

        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteWritingTestById([FromRoute] int id)
        {
            dynamic result = null;
            dynamic response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var writingTest = context.Writing_Test.Where(e => e.writingTest_id == id).FirstOrDefault();
                    response = context.Writing_Test.Remove(writingTest).ToString();
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
        public JsonResult PutWritingTestById([FromRoute] int id, [FromBody] Writing_Test writingTestData)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var writingTest = context.Writing_Test.Where(e => e.writingTest_id == id).FirstOrDefault();
                    if (writingTest == null) return new JsonResult(result);
                    writingTest.writingTest_score = writingTestData.writingTest_score ?? writingTest.writingTest_score;
                    writingTest.staff_id = writingTestData.staff_id ??writingTest.staff_id ;
                    writingTest.res_id = writingTestData.res_id??writingTest.res_id;


                    response = context.Writing_Test.Update(writingTest).ToString();
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
        [HttpGet("[action]/{id}")]
        public JsonResult GetWritingTestById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
            var data = (from wt in context.Writing_Test join s in context.Staff on wt.staff_id equals s.Staff_id into joinData from wts in joinData.DefaultIfEmpty() join rfn in context.ReservationForNow on wt.res_id equals rfn.res_id into joinData2 from wtrfn in joinData2.DefaultIfEmpty() select new { wt.writingTest_id, wt.writingTest_score, staff = wts, reservationForNow = wtrfn });
                result = new
                {
                    response = data.Where(e => e.writingTest_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }
    }

}
