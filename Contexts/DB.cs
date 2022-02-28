using Microsoft.EntityFrameworkCore;
namespace driver_app_api
{
    public class DB : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Driving_License> Driving_License { get; set; }
        public DbSet<Driving_Test> Driving_Test { get; set; }
        public DbSet<ReservationForNow> ReservationForNow { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Writing_Test> Writing_Test { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<RenewLicense> RenewLicense { get; set; }
        IConfiguration _configuration;
        public DB(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("Database"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.Role_id);
                entity.Property(e => e.Role_name);
                entity.Property(e => e.Role_description);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.User_id);
                entity.Property(e => e.Firstname);
                entity.Property(e => e.Lastname);
                entity.Property(e => e.Email);
                entity.Property(e => e.CitizenId);
                entity.Property(e => e.user_Address);
                entity.Property(e => e.user_Phone);
                entity.Property(e => e.dateOfBirth);
                entity.Property(e => e.Password);
                entity.Property(e => e.Role_id);
                entity.Property(e => e.Driving_id);
            });
            modelBuilder.Entity<Driving_License>(entity =>
            {
                entity.ToTable("Driving_License");
                entity.HasKey(e => e.Driving_id);
                entity.Property(e => e.User_id);
                entity.Property(e => e.Driving_name);
                entity.Property(e => e.Location);
            });
            modelBuilder.Entity<Driving_Test>(entity =>
            {
                entity.ToTable("Driving_Test");
                entity.HasKey(e => e.drivingTest_id);
                entity.Property(e => e.drivingTest_score);
                entity.Property(e => e.staff_id);
                entity.Property(e => e.res_id);

            });
            modelBuilder.Entity<ReservationForNow>(entity =>
            {
                entity.ToTable("ReservationForNow");
                entity.HasKey(e => e.res_id);
                entity.Property(e => e.res_date);
                entity.Property(e => e.User_id);
                entity.Property(e => e.booking_id);
                entity.Property(e => e.service);
            });
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff");
                entity.HasKey(e => e.Staff_id);
                entity.Property(e => e.Staff_name);
                entity.Property(e => e.Staff_lastname);
                entity.Property(e => e.Staff_phone);

            });
            modelBuilder.Entity<Writing_Test>(entity =>
            {
                entity.ToTable("Writing_Test");
                entity.HasKey(e => e.writingTest_id);
                entity.Property(e => e.writingTest_score);
                entity.Property(e => e.staff_id);
                entity.Property(e => e.res_id);
            });
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");
                entity.HasKey(e => e.id);
                entity.Property(e => e.date);
                entity.Property(e => e.start_time);
                entity.Property(e => e.end_time);
            });
            modelBuilder.Entity<RenewLicense>(entity =>
            {
                entity.ToTable("RenewLicense");
                entity.HasKey(e => e.user_id);
                entity.Property(e => e.name);
                entity.Property(e => e.email);
                entity.Property(e => e.phone);
                entity.Property(e => e.citizenId);
                entity.Property(e => e.drivingId);
                entity.Property(e => e.dateOfBirth);
            });
        }
    }
}
