namespace driver_app_api
{
    public class Booking
    {
        public int id { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan? start_time { get; set; }
        public TimeSpan? end_time { get; set; }

    }
}
