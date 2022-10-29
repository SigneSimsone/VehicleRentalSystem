using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Model { get; set; }
        public int Year { get; set; }

        public BrandModel Brand { get; set; }
        public LocationModel Location { get; set; }

        public CarModel()
        {
            Id = Guid.NewGuid();
        }
    }
}