namespace driver_app_api
{
    public class ReservationForNow
    {
        public int res_id { get; set; }
        public DateTime? res_date { get; set; }
        public string? service { get; set; }
        public int? User_id { get; set; }
        public int? booking_id {get;set;}
    }
}
