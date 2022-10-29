using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class ReservationModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public UserModel User { get; set; }
        public BillModel Bill { get; set; }
        public CarModel Car { get; set; }

        public ReservationModel()
        {
            Id = Guid.NewGuid();
        }
    }
}