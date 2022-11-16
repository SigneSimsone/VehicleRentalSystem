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
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
        public int FuelConsumption { get; set; }
        public int Mileage { get; set; }
        public int Passengers { get; set; }
        public int Luggage { get; set; }
        public int Doors { get; set; }
        public bool AirConditioner { get; set; }
        public float DailyPrice { get; set; }
        public bool Availability { get; set; }
        #endregion


        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
    }
}
