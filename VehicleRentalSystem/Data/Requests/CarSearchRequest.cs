using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data.Requests
{
    public class CarSearchRequest
    {
        public BrandModel Brand { get; set; }
        public GearboxModel GearboxType { get; set; }
        public FuelTypeModel FuelType { get; set; }
        public LocationModel Location { get; set; }

        public int Year { get; set; }
        public int FuelConsumption { get; set; }
        public int Mileage { get; set; }
        public int Passengers { get; set; }
        public float DailyPrice { get; set; }
    }
}
