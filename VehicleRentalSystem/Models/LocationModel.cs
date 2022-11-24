using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class LocationModel
    {
        [Key]
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }

        public LocationModel()
        {
            Id = Guid.NewGuid();
        }

        public string FullLocation
        {
            get
            {
                return $"{City}, {Street} {Number}";
            }
        }
    }
}