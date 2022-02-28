using Microsoft.AspNetCore.Mvc;

namespace driver_app_api.Controllers
{
    [Controller]
    [Route("Booking")]
    public class BookingController : Controller
    {
        IConfiguration _configuration;
        public BookingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public JsonResult GetBookingList()
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // SELECT  dl.Driving_id,dl.User_id,dl.Driving_name,dl.Location,user=dlu from Driving_License dl join user u on dl.user_id = u.user_id
                dynamic response = (from b in context.Booking
                                    select new
                                    {
                                        b.id,
                                        b.date,
                                        b.start_time,
                                        b.end_time
                                    }).ToList();
                result = new
                {
                    response
                };
            }

            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetBookingById([FromRoute] int id)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                // Select dl.Driving_id,dl.User_id, dl.Driving_name,dl.Location,u.* from Driving_License dl join user u on dl.user_id = u.user_id
                var data = (from b in context.Booking
                            select new
                            {
                                b.id,
                                b.date,
                                b.start_time,
                                b.end_time
                            });
                result = new
                {
                    response = data.Where(e => e.id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);
        }
        [HttpPost("[action]")]
        public JsonResult PostBooking([FromBody] Booking bookingData)
        {
            dynamic result = null;
            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    // insert into Driving_License values(?,?,?)
                    response = context.Booking.Add(bookingData).ToString();
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
        public JsonResult PutDrivingLicenseById([FromRoute] int id, [FromBody] Booking bookingData)
        {
            dynamic? result = null;
            dynamic? response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    // update Driving_License set ? = ? where ;
                    var booking = context.Booking.Where(e => e.id == id).FirstOrDefault();
                    if (booking == null) return new JsonResult(result);
                    booking.date = bookingData.date ?? booking.date;
                    booking.start_time = bookingData.start_time ?? booking.start_time;
                    booking.end_time = bookingData.end_time ?? booking.end_time;
                    response = context.Booking.Update(booking).ToString();
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
                    var booking = context.Booking.Where(e => e.id == id).FirstOrDefault();
                    response = context.Booking.Remove(booking).ToString();
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
