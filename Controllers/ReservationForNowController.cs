using Microsoft.AspNetCore.Mvc;

namespace driver_app_api
{
    [Controller]
    [Route("ReservationForNow")]
    public class ReservationForNowController : Controller
    {

        IConfiguration _configuration;

        public ReservationForNowController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public JsonResult GetReservationForNowList()
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                result = new
                {
                    response = (from rfn in context.ReservationForNow
                                join u in context.User on rfn.User_id equals u.User_id into joinData
                                from rfnu in joinData.DefaultIfEmpty()
                                join b in context.Booking on rfn.booking_id equals b.id into bookingData
                                from bdata in bookingData.DefaultIfEmpty()
                                select new
                                {
                                    rfn.res_id,
                                    rfn.res_date,
                                    rfn.service,
                                    rfn.booking_id,
                                    user = rfnu,
                                    booking = bdata
                                }).ToList()
                };
            }
            return new JsonResult(result);
        }
        [HttpPost("[action]")]
        public JsonResult PostReservationForNow([FromBody] ReservationForNow reservationForNowData)
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                dynamic response = null;
                try
                {
                    response = context.ReservationForNow.Add(reservationForNowData).ToString();
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
                result = new
                {
                    response,
                    data = reservationForNowData
                };
            }

            return new JsonResult(result);

        }
        [HttpDelete("[action]/{id}")]
        public JsonResult DeleteReservationForNow([FromRoute] int id)
        {
            dynamic? result = null;
            dynamic? response = null;
            var reservationForNow = new ReservationForNow();
            using (var context = new DB(_configuration))
            {
                try
                {
                    reservationForNow = context.ReservationForNow.Where(e => e.res_id == id).FirstOrDefault();
                    response = context.ReservationForNow.Remove(reservationForNow).ToString();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                }
                result = new
                {
                    response,
                    data=reservationForNow
                };
            }

            return new JsonResult(result);
        }
        [HttpGet("[action]/{id}")]
        public JsonResult GetReservationForNowById([FromRoute] int id)
        {
            dynamic result = null;

            using (var context = new DB(_configuration))
            {
                var data = (from rfn in context.ReservationForNow
                            join u in context.User on rfn.User_id equals u.User_id into joinData
                            from rfnu in joinData.DefaultIfEmpty()
                            join b in context.Booking on rfn.booking_id equals b.id into bookingData
                            from bdata in bookingData.DefaultIfEmpty()
                            select new
                            {
                                rfn.res_id,
                                rfn.res_date,
                                rfn.service,
                                rfn.booking_id,
                                booking = bdata,
                                user = rfnu
                            });
                result = new
                {
                    response = data.Where(e => e.res_id == id).FirstOrDefault()
                };
            }

            return new JsonResult(result);

        }

        [HttpPut("[action]/{id}")]
        public JsonResult PutReservationForNowById([FromRoute] int id, [FromBody] ReservationForNow reservationForNowData)
        {
            dynamic result = null;
            dynamic response = null;
            using (var context = new DB(_configuration))
            {
                try
                {
                    var reservationForNow = context.ReservationForNow.Where(e => e.res_id == id).FirstOrDefault();
                    if (reservationForNow == null) return new JsonResult(result);
                    reservationForNow.res_date = reservationForNowData.res_date ?? reservationForNow.res_date;
                    reservationForNow.User_id = reservationForNowData.User_id ?? reservationForNow.User_id;
                    reservationForNow.service = reservationForNowData.service ?? reservationForNow.service;
                    reservationForNow.booking_id = reservationForNowData.booking_id ?? reservationForNow.booking_id;

                    response = context.ReservationForNow.Update(reservationForNow).ToString();
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
