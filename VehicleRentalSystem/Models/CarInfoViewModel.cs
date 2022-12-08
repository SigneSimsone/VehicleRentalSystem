using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarInfoViewModel
    {
        public BrandModel[] Brands { get; set; }
        public CarModelModel[] Models { get; set; }
        public FuelTypeModel[] FuelTypes { get; set; }
        public GearboxModel[] GearboxTypes { get; set; }
        public LocationModel[] Locations { get; set; }


        [Required(ErrorMessage = "The field Brand is required.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "The field Car Model is required.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "The field Fuel Type is required.")]
        public string FuelType { get; set; }

        [Required(ErrorMessage = "The field Gearbox Type is required.")]
        public string Gearbox { get; set; }

        [Required(ErrorMessage = "The field City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The field Street is required.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "The field Number is required.")]
        public string Number { get; set; }

        public string FullLocation
        {
            get
            {
                return $"{City}, {Street} {Number}";
            }
        }
    }
}
