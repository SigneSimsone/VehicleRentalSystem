using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class ReservationViewModel
    {
        public Guid CarId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public string Brand { get; set; }
        public string CarModel { get; set; }
        public string GearboxType { get; set; }
        public string FuelType { get; set; }
        public string Location { get; set; }
        public float DailyPrice { get; set; }

        public CarModel Car { get; set; }
        public string UserId { get; set; }

        public PaymentModel Payment { get; set; }
        public double Amount { get; set; }
    }
}
