using Microsoft.AspNetCore.Mvc;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly CarDataManager _carDataManager;

        public CarController(CarDataManager carDataManager)
        {
            _carDataManager = carDataManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            string relativeFilePath = null;
            BrandModel brandmodel = _carDataManager.GetOneBrand(model.SelectedBrand);
            CarModelModel carmodelmodel = _carDataManager.GetOneCarModel(model.SelectedCarModel);
            GearboxModel gearboxmodel = _carDataManager.GetOneGearboxType(model.SelectedGearboxType);
            FuelTypeModel fueltypemodel = _carDataManager.GetOneFuelType(model.SelectedFuelType);
            LocationModel locationmodel = _carDataManager.GetOneLocation(model.SelectedLocation);

            if (model.File != null)
            {
                //upload files to wwwroot
                string fileName = $"{brandmodel.Brand}_{carmodelmodel.Model}.jpg".Replace(" ", "");
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "images", fileName);

                using (var fileSteam = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileSteam);
                }

                relativeFilePath = Path.Combine("\\images", fileName);
            }

            _carDataManager.AddCar(brandmodel, carmodelmodel, gearboxmodel, fueltypemodel, model.Year, model.RegistrationNumber, model.FuelConsumption, model.Mileage, model.Passengers, model.Luggage, model.Doors, model.AirConditioner, model.Availability, model.DailyPrice, relativeFilePath, locationmodel);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult AddBrand(string brand)
        {
            if(brand == null)
            {
               
            }
            _carDataManager.AddBrand(brand);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddFuelType(string fuelType)
        {
            _carDataManager.AddFuelType(fuelType);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddGearboxType(string gearbox)
        {
            _carDataManager.AddGearboxType(gearbox);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddCarModel(string model)
        {
            _carDataManager.AddCarModel(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddLocation(string city, string street, string number)
        {
            _carDataManager.AddLocation(city, street, number);

            return RedirectToAction(nameof(Index));
        }
    }
}
