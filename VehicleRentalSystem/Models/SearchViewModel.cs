using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class SearchViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SelectList BrandDropdown { get; set; }
        public SelectList GearboxTypeDropdown { get; set; }
        public SelectList FuelTypeDropdown { get; set; }
        public SelectList LocationDropdown { get; set; }

        public Guid? SelectedBrand { get; set; }
        public Guid? SelectedGearboxType { get; set; }
        public Guid? SelectedFuelType { get; set; }
        public Guid? SelectedLocation { get; set; }

        [RegularExpression("^[0-9]{4}$", ErrorMessage = "The field Year must contain exactly 4 numbers.")]
        public int? Year { get; set; }

        [RegularExpression("^[0-9]+[0-9,]*$", ErrorMessage = "Must input a positive number, decimal separator must be a comma.")]
        public float? FuelConsumption { get; set; }

        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number.")]
        public int? Mileage { get; set; }

        [RegularExpression("^[0-9]+[0-9]*$", ErrorMessage = "Must input a positive number.")]
        public int? Passengers { get; set; }

        [RegularExpression("^[0-9]+[0-9,]*$", ErrorMessage = "Must input a positive number, decimal separator must be a comma.")]
        public decimal? DailyPrice { get; set; }
    }
}
