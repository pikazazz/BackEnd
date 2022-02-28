namespace driver_app_api
{
    public class RenewLicense
    {
        public int user_id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public int? citizenId { get; set; }
        public int? driverId { get; set; }
        public DateTime? dateOfBirth { get; set; }

    }
}
