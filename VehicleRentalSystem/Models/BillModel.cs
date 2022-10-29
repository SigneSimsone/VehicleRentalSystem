using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class BillModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }

        public ReservationModel Reservation { get; set; }

        public BillModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
