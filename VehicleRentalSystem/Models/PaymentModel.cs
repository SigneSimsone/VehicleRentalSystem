using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class PaymentModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }
        public double Amount { get; set; }

        public virtual ReservationModel Reservation { get; set; }

        public PaymentModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
