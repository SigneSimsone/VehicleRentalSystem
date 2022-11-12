using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class FuelTypeModel
    {
        [Key]
        public Guid Id { get; set; }

        public string FuelType { get; set; }

        public FuelTypeModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
