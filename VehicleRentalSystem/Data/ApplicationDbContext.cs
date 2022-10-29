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
 /*       public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<ArtworkModel> Artworks { get; set; }
        public DbSet<StyleModel> Styles { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }
        public DbSet<ExhibitionModel> Exhibitions { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<AuditModel> Audits { get; set; }*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
