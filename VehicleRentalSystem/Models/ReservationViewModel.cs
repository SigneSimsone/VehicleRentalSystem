using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class ReservationViewModel
    {
        public Guid ReservationId { get; set; }
        public Guid PaymentId { get; set; }

        public ReservationModel[] Reservations { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }

        public Guid CarId { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime EndDate { get; set; }

        public string StartDateString { get; set; }
        public string EndDateString { get; set; }

        public string Brand { get; set; }
        public string CarModel { get; set; }
        public string GearboxType { get; set; }
        public string FuelType { get; set; }
        public string Location { get; set; }
        public float DailyPrice { get; set; }

        public double Amount { get; set; }

        public string SuccessMessage { get; set; }
    }
}
