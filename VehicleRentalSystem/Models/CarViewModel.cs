using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarViewModel
    {
        public Guid Id { get; set; }

        public CarModel[] Cars { get; set; }
        public string UserId { get; set; }

        public SelectList BrandDropdown { get; set; }
        public SelectList CarModelDropdown { get; set; }
        public SelectList GearboxTypeDropdown { get; set; }
        public SelectList FuelTypeDropdown { get; set; }
        public SelectList LocationDropdown { get; set; }
        public SelectList AvailabilityDropdown { get; set; }
        public int SelectedAvailability { get; set; }


        #region NewArtworkFields
        [Required(ErrorMessage = "Brand field error")]
        public Guid SelectedBrand { get; set; }

        [Required(ErrorMessage = "Model field error")]
        public Guid SelectedCarModel { get; set; }

        [Required(ErrorMessage = "Gearbox type field error")]
        public Guid SelectedGearboxType { get; set; }

        [Required(ErrorMessage = "Fuel type field error")]
        public Guid SelectedFuelType { get; set; }

        [Required(ErrorMessage = "Location field error")]
        public Guid SelectedLocation { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public int FuelConsumption { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public int Passengers { get; set; }
        [Required]
        public int Luggage { get; set; }

        [Required]
        public int Doors { get; set; }
        [Required]
        public bool AirConditioner { get; set; }

        [Required(ErrorMessage = "Daily Price field error")]
        public float DailyPrice { get; set; }
        [Required]
        public bool Availability { get; set; }
        #endregion


        [Required]
        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
    }
}
