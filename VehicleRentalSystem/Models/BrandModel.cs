using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class BrandModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Brand { get; set; }

        public BrandModel()
        {
            Id = Guid.NewGuid();
        }
    }
}