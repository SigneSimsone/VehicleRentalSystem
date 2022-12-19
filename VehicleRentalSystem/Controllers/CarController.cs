using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyfService;

        public CarController(CarDataManager carDataManager, UserManager<UserModel> userManager, IWebHostEnvironment hostingEnv, INotyfService notyfService)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
            _hostingEnv = hostingEnv;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult Index(CarViewModel viewModel = null)
        {
            CarModel[] cars = _carDataManager.GetCars();
            var userId = _userManager.GetUserId(User);

            if (viewModel == null)
            {
                viewModel = new CarViewModel();
            }
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            CarModel[] cars = _carDataManager.GetCarsByRegNr(model.RegistrationNumber);

            if (cars.Any())
            {
                _notyfService.Error("Car with the same registration number already exists!");
                return View("AddCar", model);
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
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "img", fileName);

                using (var fileSteam = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileSteam);
                }

                relativeFilePath = Path.Combine("\\img", fileName);
            }

            _carDataManager.AddCar(brandmodel, carmodelmodel, gearboxmodel, fueltypemodel, model.Year.Value, model.RegistrationNumber, model.FuelConsumption.Value, model.Mileage.Value, model.Passengers.Value, model.Luggage.Value, model.Doors.Value, model.AirConditioner, model.Availability, model.DailyPrice.Value, relativeFilePath, locationmodel);
            _notyfService.Success("Car added successfully!");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid CarId, CarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            CarModel[] cars = _carDataManager.GetCarsByRegNr(model.RegistrationNumber);

            if (cars.Any())
            {
                _notyfService.Error("Car with the same registration number already exists!");
                return RedirectToAction(nameof(Edit), model);
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
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "img", fileName);

                using (var fileSteam = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileSteam);
                }

                relativeFilePath = Path.Combine("\\img", fileName);
            }

            _carDataManager.Edit(CarId, brandmodel, carmodelmodel, gearboxmodel, fueltypemodel, model.Year.Value, model.RegistrationNumber, model.FuelConsumption.Value, model.Mileage.Value, model.Passengers.Value, model.Luggage.Value, model.Doors.Value, model.AirConditioner, model.Availability, model.DailyPrice.Value, relativeFilePath, locationmodel);
            _notyfService.Success("Car edited successfully!");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid CarId)
        {
            // get car from database (CarModel)
            CarModel car = _carDataManager.GetOneCar(CarId);
            CarViewModel viewModel = new CarViewModel();
            viewModel.CarId = car.Id;

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand", car.Brand.Brand);
			viewModel.SelectedBrand = car.Brand.Id;

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model", car.Model.Model);
			viewModel.SelectedCarModel = car.Model.Id;

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox", car.GearboxType.Gearbox);
            viewModel.SelectedGearboxType = car.GearboxType.Id;

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType", car.FuelType.FuelType);
            viewModel.SelectedFuelType = car.FuelType.Id;

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation", car.Location.FullLocation);
            viewModel.SelectedLocation = car.Location.Id;

            viewModel.Year = car.Year;
            viewModel.RegistrationNumber = car.RegistrationNumber;
            viewModel.FuelConsumption = car.FuelConsumption;
            viewModel.Mileage = car.Mileage;
            viewModel.Passengers = car.Passengers;
            viewModel.Luggage = car.Luggage;
            viewModel.Doors = car.Doors;
            viewModel.AirConditioner = car.AirConditioner;
            viewModel.Availability = car.Availability;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.ImagePath = car.ImagePath;

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
            ReservationModel[] reservations = _carDataManager.GetReservations(CarId);

            if (reservations.Any())
            {
                _notyfService.Error("Cannot delete a car with active reservations!");
                return RedirectToAction(nameof(Index));
            }

            _carDataManager.Delete(CarId);
            _notyfService.Success("Car deleted successfully!");

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

            if (!cars.Any())
            {
                ModelState.AddModelError("NoCarFoundMessage", "No cars were found that match your criteria!");
            }

            return View("Index", viewModel);
        }


        [HttpGet]
        public IActionResult OpenAddCar(CarViewModel viewModel = null)
        {
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

            viewModel.FuelConsumption = null;

            return View("AddCar", viewModel);
        }

        [HttpGet]
        public IActionResult OpenAddCarProperties()
        {
            return View("AddCarProperties");
        }

        [HttpPost]
        public IActionResult AddBrand(string Brand)
        {
            BrandModel[] brands = _carDataManager.GetBrands();

            if (brands.Any(x => string.Equals(x.Brand, Brand, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This brand already exists in the system!");
                return View("AddCarProperties");
            }
            _carDataManager.AddBrand(Brand);
            _notyfService.Success("Brand added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddCarModel(string CarModel)
        {
            CarModelModel[] carModels = _carDataManager.GetCarModels();
            if (carModels.Any(x => string.Equals(x.Model, CarModel, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This model already exists in the system!");
                return View("AddCarProperties");
            }
            _carDataManager.AddCarModel(CarModel);
            _notyfService.Success("Model added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddFuelType(string FuelType)
        {
            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            if (fuelTypes.Any(x => string.Equals(x.FuelType, FuelType, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This fuel type already exists in the system!");
                return View("AddCarProperties");
            }
            _carDataManager.AddFuelType(FuelType);
            _notyfService.Success("Fuel type added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddGearboxType(string Gearbox)
        {
            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            if (gearboxTypes.Any(x => string.Equals(x.Gearbox, Gearbox, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This gearbox type already exists in the system!");
                return View("AddCarProperties");
            }
            _carDataManager.AddGearboxType(Gearbox);
            _notyfService.Success("Gearbox type added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddLocation(string City, string Street, string Number)
        {
            LocationModel[] locations = _carDataManager.GetLocations();
            if (locations.Any(x =>
            string.Equals(x.City, City, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Street, Street, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Number, Number, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This location already exists in the system!");
                return View("AddCarProperties");
            }
            _carDataManager.AddLocation(City, Street, Number);
            _notyfService.Success("Location added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult ShowCarProperties(CarPropertiesViewModel viewModel = null)
        {
            BrandModel[] brands = _carDataManager.GetBrands();
            CarModelModel[] models = _carDataManager.GetCarModels();
            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            LocationModel[] locations = _carDataManager.GetLocations();


            if (viewModel == null)
            {
                viewModel = new CarPropertiesViewModel();
            }
            viewModel.Brands = brands;
            viewModel.Models = models;
            viewModel.FuelTypes = fuelTypes;
            viewModel.GearboxTypes = gearboxTypes;
            viewModel.Locations = locations;

            return View("ShowCarProperties", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditBrand(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}
            var existingBrand = _carDataManager.GetOneBrand(model.BrandId);

            BrandModel[] brands = _carDataManager.GetBrands();
            if (brands.Any(x => string.Equals(x.Brand, model.Brand, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingBrand.Brand, model.Brand, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This brand already exists in the system!");
                    return View(model);
                }
            }
            _carDataManager.EditBrand(model.BrandId, model.Brand);
            _notyfService.Success("Brand edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditBrand(Guid BrandId)
        {
            // get brand from database
            BrandModel brand = _carDataManager.GetOneBrand(BrandId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.BrandId = brand.Id;
            viewModel.Brand = brand.Brand;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}
            var existingModel = _carDataManager.GetOneCarModel(model.CarModelId);

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            if (carModels.Any(x => string.Equals(x.Model, model.CarModel, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingModel.Model, model.CarModel, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This model already exists in the system!");
                    return View(model);
                }
            }
            _carDataManager.EditCarModel(model.CarModelId, model.CarModel);
            _notyfService.Success("Model edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditModel(Guid CarModelId)
        {
            // get car model from database
            CarModelModel carModel = _carDataManager.GetOneCarModel(CarModelId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.CarModelId = carModel.Id;
            viewModel.CarModel = carModel.Model;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFuelType(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}
            var existingFuelType = _carDataManager.GetOneFuelType(model.FuelTypeId);

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            if (fuelTypes.Any(x => string.Equals(x.FuelType, model.FuelType, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingFuelType.FuelType, model.FuelType, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This fuel type already exists in the system!");
                    return View(model);
                }
            }
            _carDataManager.EditFuelType(model.FuelTypeId, model.FuelType);
            _notyfService.Success("Fuel type edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditFuelType(Guid FuelTypeId)
        {
            // get fuel type from database
            FuelTypeModel fuelType = _carDataManager.GetOneFuelType(FuelTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.FuelTypeId = fuelType.Id;
            viewModel.FuelType = fuelType.FuelType;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditGearbox(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}
            var existingGearboxType = _carDataManager.GetOneGearboxType(model.GearboxId);

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            if (gearboxTypes.Any(x => string.Equals(x.Gearbox, model.Gearbox, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingGearboxType.Gearbox, model.Gearbox, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This gearbox type already exists in the system!");
                    return View(model);
                }
            }
            _carDataManager.EditGearboxType(model.GearboxId, model.Gearbox);
            _notyfService.Success("Gearbox type edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditGearbox(Guid GearboxTypeId)
        {
            // get gearbox type from database
            GearboxModel gearbox = _carDataManager.GetOneGearboxType(GearboxTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.GearboxId = gearbox.Id;
            viewModel.Gearbox = gearbox.Gearbox;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}
            var existingLocation = _carDataManager.GetOneLocation(model.LocationId);

            LocationModel[] locations = _carDataManager.GetLocations();
            if (locations.Any(x =>
            string.Equals(x.City, model.City, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Street, model.Street, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Number, model.Number, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingLocation.FullLocation, model.FullLocation, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This location already exists in the system!");
                    return View(model);
                }
            }
            _carDataManager.EditLocation(model.LocationId, model.City, model.Street, model.Number);
            _notyfService.Success("Location edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditLocation(Guid LocationId)
        {
            // get location from database
            LocationModel location = _carDataManager.GetOneLocation(LocationId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.LocationId = location.Id;
            viewModel.City = location.City;
            viewModel.Street = location.Street;
            viewModel.Number = location.Number;

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult DeleteBrand(Guid BrandId)
        {
            CarModel[] cars = _carDataManager.GetCarsByBrand(BrandId);

            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a brand with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteBrand(BrandId);
            _notyfService.Success("Brand deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteModel(Guid CarModelId)
        {
            CarModel[] cars = _carDataManager.GetCarsByCarModel(CarModelId);

            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a model with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteCarModel(CarModelId);
            _notyfService.Success("Model deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteFuelType(Guid FuelTypeId)
        {
            CarModel[] cars = _carDataManager.GetCarsByFuelType(FuelTypeId);

            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a fuel type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteFuelType(FuelTypeId);
            _notyfService.Success("Fuel type deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteGearbox(Guid GearboxTypeId)
        {
            CarModel[] cars = _carDataManager.GetCarsByGearboxType(GearboxTypeId);

            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a gearbox type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteGearboxType(GearboxTypeId);
            _notyfService.Success("Gearbox type deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteLocation(Guid LocationId)
        {
            CarModel[] cars = _carDataManager.GetCarsByLocation(LocationId);

            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a location with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteLocation(LocationId);
            _notyfService.Success("Location deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }


        [HttpPost]
        public IActionResult AddFeedback(Guid CarId, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                _notyfService.Error("Please input your comment!");
                return RedirectToAction(nameof(OpenCar), new { CarId });
            }
            var user = _userManager.GetUserAsync(User).Result;
            _carDataManager.AddFeedback(CarId, comment, user);
            _notyfService.Success("Feedback added successfully!");

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }

        [HttpPost]
        public IActionResult DeleteFeedback(Guid FeedbackId, Guid CarId)
        {
            _carDataManager.DeleteFeedback(FeedbackId);
            _notyfService.Success("Feedback deleted successfully!");

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }
    }
}
