using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class GearboxModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Gearbox { get; set; }

        public GearboxModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
