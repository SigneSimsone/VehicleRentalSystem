using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class LocationModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Location { get; set; }

        public LocationModel()
        {
            Id = Guid.NewGuid();
        }
    }
}