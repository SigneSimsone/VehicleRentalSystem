using Microsoft.AspNetCore.Mvc.Rendering;

namespace VehicleRentalSystem.Models
{
    public class SearchViewModel
    {
        public Guid Id { get; set; }

        public SelectList BrandDropdown { get; set; }
        public SelectList CarModelDropdown { get; set; }
        public SelectList GearboxTypeDropdown { get; set; }
        public SelectList FuelTypeDropdown { get; set; }
        public SelectList LocationDropdown { get; set; }
        public SelectList AvailabilityDropdown { get; set; }
        public SelectList AirConditionerDropdown { get; set; }

        public Guid SelectedBrand { get; set; }
        public Guid SelectedCarModel { get; set; }
        public Guid SelectedGearboxType { get; set; }
        public Guid SelectedFuelType { get; set; }
        public Guid SelectedLocation { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
        public int FuelConsumption { get; set; }
        public int Mileage { get; set; }
        public int Passengers { get; set; }
        public int Luggage { get; set; }
        public int Doors { get; set; }
        public float DailyPrice { get; set; }
        public int SelectedAirConditioner { get; set; }
        public int SelectedAvailability { get; set; }
    }
}
