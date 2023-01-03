using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarViewModel
    {
        public Guid CarId { get; set; }

        public CarModel[]? Cars { get; set; }
        public string? UserId { get; set; }

        public SelectList? BrandDropdown { get; set; }
        public SelectList? CarModelDropdown { get; set; }
        public SelectList? GearboxTypeDropdown { get; set; }
        public SelectList? FuelTypeDropdown { get; set; }
        public SelectList? LocationDropdown { get; set; }


        #region NewCarFields
        [Required(ErrorMessage = "The field Brand is required.")]
        public Guid SelectedBrand { get; set; }

        [Required(ErrorMessage = "The field Model is required.")]
        public Guid SelectedCarModel { get; set; }

        [Required(ErrorMessage = "The field Gearbox type is required.")]
        public Guid SelectedGearboxType { get; set; }

        [Required(ErrorMessage = "The field Fuel type is required.")]
        public Guid SelectedFuelType { get; set; }

        [Required(ErrorMessage = "The field Location is required.")]
        public Guid SelectedLocation { get; set; }

        [Required(ErrorMessage = "The field Year is required.")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "The field Year must contain exactly 4 numbers")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "The field Registration Number is required.")]
        public string? RegistrationNumber { get; set; }

        [Required(ErrorMessage = "The field Fuel Consumption is required.")]
        [RegularExpression("^[0-9]+[0-9,.]*$", ErrorMessage = "Must input a positive number")]
        public float? FuelConsumption { get; set; }

        [Required(ErrorMessage = "The field Mileage is required.")]
        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number")]
        public int? Mileage { get; set; }

        [Required(ErrorMessage = "The field Passengers is required.")]
        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number")]
        public int? Passengers { get; set; }

        [Required(ErrorMessage = "The field Luggage is required.")]
        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number")]
        public int? Luggage { get; set; }

        [Required(ErrorMessage = "The field Doors is required.")]
        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number")]
        public int? Doors { get; set; }

        [Required(ErrorMessage = "The field Air Conditioner is required.")]
        public bool AirConditioner { get; set; }

        [Required(ErrorMessage = "The field Daily Price is required.")]
        [RegularExpression("^[0-9]+[0-9,.]*$", ErrorMessage = "Must input a positive number")]
        public decimal? DailyPrice { get; set; }

        [Required(ErrorMessage = "The field Availability is required.")]
        public bool Availability { get; set; }

        public IFormFile? File { get; set; }
        public string? ImagePath { get; set; }
        #endregion
    }
}
