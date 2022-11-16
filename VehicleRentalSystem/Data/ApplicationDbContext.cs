using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<FuelTypeModel> FuelTypes { get; set; }
        public DbSet<GearboxModel> GearboxTypes { get; set; }
        public DbSet<CarModelModel> CarModels { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }
        public DbSet<ReservationModel> Reservations { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PaymentModel>()
        .HasOne(a => a.Reservation).WithOne(b => b.Payment)
        .HasForeignKey<ReservationModel>(e => e.PaymentId);
        }
    }
}
