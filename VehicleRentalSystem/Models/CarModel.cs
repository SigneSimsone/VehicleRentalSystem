using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
        public int FuelConsumption { get; set; }
        public int Mileage { get; set; }
        public int Passengers { get; set; }
        public int Luggage { get; set; }
        public int Doors { get; set; }
        public bool AirConditioner { get; set; }
        public bool Availability { get; set; }
        public float DailyPrice { get; set; }
        public string ImagePath { get; set; }

        public BrandModel Brand { get; set; }
        public FuelTypeModel FuelType { get; set; }
        public GearboxModel GearboxType { get; set; }
        public CarModelModel Model { get; set; }
        public LocationModel Location { get; set; }

        public ICollection<FeedbackModel> Feedbacks { get; set; }
        public ICollection<ReservationModel> Reservations { get; set; }

        public CarModel()
        {
            Id = Guid.NewGuid();
        }
    }
}