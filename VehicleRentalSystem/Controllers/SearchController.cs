using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Data.Requests;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly CarDataManager _carDataManager;

        public SearchController(CarDataManager carDataManager)
        {
            _carDataManager = carDataManager;
        }

        public IActionResult Index()
        {
            SearchViewModel viewModel = new SearchViewModel();

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand");

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox");

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType");

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchCars(SearchViewModel model)
        {

            BrandModel brandmodel = null;
            GearboxModel gearboxmodel = null;
            FuelTypeModel fueltypemodel = null;
            LocationModel locationmodel = null;

            if (model.SelectedBrand != Guid.Empty)
            {
                brandmodel = _carDataManager.GetOneBrand(model.SelectedBrand);

            }
            if (model.SelectedGearboxType != Guid.Empty)
            {
                gearboxmodel = _carDataManager.GetOneGearboxType(model.SelectedGearboxType);

            }
            if (model.SelectedFuelType != Guid.Empty)
            {
                fueltypemodel = _carDataManager.GetOneFuelType(model.SelectedFuelType);

            }
            if (model.SelectedLocation != Guid.Empty)
            {
                locationmodel = _carDataManager.GetOneLocation(model.SelectedLocation);

            }

            CarSearchRequest requestModel = new CarSearchRequest()
            {
                Brand = brandmodel,
                GearboxType = gearboxmodel,
                FuelType = fueltypemodel,
                Location = locationmodel,
                Year = model.Year,
                FuelConsumption = model.FuelConsumption,
                Mileage = model.Mileage,
                Passengers = model.Passengers,
                DailyPrice = model.DailyPrice,
            };

            CarModel[] cars = _carDataManager.SearchCars(requestModel);

            Guid[] carIdList = cars.Select(x => x.Id).ToArray();

            return RedirectToAction("ShowFilteredCars", "Car", new { carsId = carIdList });
        }
    }
}
