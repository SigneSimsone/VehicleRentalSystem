using Microsoft.AspNetCore.Mvc.Rendering;

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
        public int? Year { get; set; }
        public int? FuelConsumption { get; set; }
        public int? Mileage { get; set; }
        public int? Passengers { get; set; }
        public float? DailyPrice { get; set; }
    }
}
