using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly CarDataManager _carDataManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _hostingEnv;

        public CarController(CarDataManager carDataManager, UserManager<UserModel> userManager, IWebHostEnvironment hostingEnv)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
            _hostingEnv = hostingEnv;
        }

        [HttpGet]
        public IActionResult Index(CarViewModel viewModel = null)
        {
            CarModel[] cars = _carDataManager.GetCars();
            var userId = _userManager.GetUserId(User);

            if(viewModel == null)
            {
                viewModel = new CarViewModel();
            }
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand");

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model");

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
        public async Task<IActionResult> AddCar(CarViewModel model)
        {
           // if (!ModelState.IsValid)
            //{
           //     return RedirectToAction(nameof(Index), model);
          //  }

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
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "img", fileName);

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
        public async Task<IActionResult> Edit(Guid CarId, CarViewModel model)
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

        [HttpGet]
        public IActionResult Edit(Guid CarId)
        {
            // get car from database (CarModel)
            CarModel model = _carDataManager.GetOneCar(CarId);
            CarViewModel viewModel = new CarViewModel();
            viewModel.Id = model.Id;

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand", model.Brand.Brand);

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model", model.Model.Model);

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox", model.GearboxType.Gearbox);

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType", model.FuelType.FuelType);

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation", model.Location.FullLocation);

            viewModel.Year = model.Year;
            viewModel.RegistrationNumber = model.RegistrationNumber;
            viewModel.FuelConsumption = model.FuelConsumption;
            viewModel.Mileage = model.Mileage;
            viewModel.Passengers = model.Passengers;
            viewModel.Luggage = model.Luggage;
            viewModel.Doors = model.Doors;
            viewModel.AirConditioner = model.AirConditioner;
            viewModel.Availability = model.Availability;
            viewModel.DailyPrice = model.DailyPrice;
            viewModel.ImagePath = model.ImagePath;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult OpenCar(Guid CarId)
        {
            if (CarId == Guid.Empty)
            {
                return RedirectToAction(nameof(Index));
            }

            // get car from database (CarModel)
            CarModel model = _carDataManager.GetOneCar(CarId);

            return View("OneCar", model);
        }

        [HttpPost]
        public IActionResult Delete(Guid CarId)
        {

            _carDataManager.Delete(CarId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ShowFilteredCars(Guid[] carsId)
        {
            var userId = _userManager.GetUserId(User);
            CarModel[] cars = _carDataManager.GetCars(carsId.ToList());

            CarViewModel viewModel = new CarViewModel();
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand");

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model");

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox");

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType");

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation");

            return View("Index", viewModel);
        }



        [HttpPost]
        public IActionResult AddBrand(string brand)
        {
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
        public IActionResult AddGearboxType(string gearboxType)
        {
            _carDataManager.AddGearboxType(gearboxType);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddCarModel(string carModel)
        {
            _carDataManager.AddCarModel(carModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddLocation(string city, string street, string number)
        {
            _carDataManager.AddLocation(city, street, number);

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public IActionResult AddFeedback(Guid CarId, string comment)
        {
            var user = _userManager.GetUserAsync(User).Result;
            _carDataManager.AddFeedback(CarId, comment, user);

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }

        [HttpPost]
        public IActionResult DeleteFeedback(Guid FeedbackId, Guid CarId)
        {
            _carDataManager.DeleteFeedback(FeedbackId);

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }
    }
}
