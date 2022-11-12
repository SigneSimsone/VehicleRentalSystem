using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarModelModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Model { get; set; }

        public CarModelModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
