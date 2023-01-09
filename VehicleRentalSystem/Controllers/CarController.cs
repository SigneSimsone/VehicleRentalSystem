using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
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

            //check if system has cars
            if (!cars.Any())
            {
                ModelState.AddModelError("NoCarsAdded", "No cars have been added in the system!");
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewModel model)
        {//add a new car
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }
            //check if reg nr does not match
            CarModel[] cars = _carDataManager.GetCarsByRegNr(model.RegistrationNumber);

            if (cars.Any())
            {
                _notyfService.Error("Car with the same registration number already exists!");
                return RedirectToAction(nameof(OpenAddCar), model);
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

            var dailyPrice = Math.Round(model.DailyPrice.Value, 2);

            //add to database
            _carDataManager.AddCar(brandmodel,
                                   carmodelmodel,
                                   gearboxmodel,
                                   fueltypemodel,
                                   model.Year.Value,
                                   model.RegistrationNumber,
                                   model.FuelConsumption.Value,
                                   model.Mileage.Value,
                                   model.Passengers.Value,
                                   model.Luggage.Value,
                                   model.Doors.Value,
                                   model.AirConditioner,
                                   model.Availability,
                                   dailyPrice,
                                   relativeFilePath,
                                   locationmodel);
            _notyfService.Success("Car added successfully!");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid CarId, CarViewModel model)
        {//save edited information
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            CarModel[] cars = _carDataManager.GetCarsByRegNr(model.RegistrationNumber);

            //check if reg nr does not match
            var existingCar = _carDataManager.GetOneCar(CarId);
            if (cars.Any(x => string.Equals(x.RegistrationNumber, model.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingCar.RegistrationNumber, model.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("Car with the same registration number already exists!");
                    return RedirectToAction(nameof(Edit), model);
                }
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

            //edit in database
            _carDataManager.Edit(CarId,
                                 brandmodel,
                                 carmodelmodel,
                                 gearboxmodel,
                                 fueltypemodel,
                                 model.Year.Value,
                                 model.RegistrationNumber,
                                 model.FuelConsumption.Value,
                                 model.Mileage.Value,
                                 model.Passengers.Value,
                                 model.Luggage.Value,
                                 model.Doors.Value,
                                 model.AirConditioner,
                                 model.Availability,
                                 model.DailyPrice.Value,
                                 relativeFilePath,
                                 locationmodel);
            _notyfService.Success("Car edited successfully!");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(Guid CarId)
        {//show car info for editing
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
        {//open more information about a car
            if (CarId == Guid.Empty)
            {
                return RedirectToAction(nameof(Index));
            }

            // get car from database (CarModel)
            CarModel model = _carDataManager.GetOneCar(CarId);

            return View("OneCar", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Guid CarId)
        {//delete a car
            ReservationModel[] reservations = _carDataManager.GetReservations(CarId);

            if (reservations.Any())
            {
                _notyfService.Error("Cannot delete a car with active reservations!");
                return RedirectToAction(nameof(Index));
            }

            //delete from database
            _carDataManager.Delete(CarId);
            _notyfService.Success("Car deleted successfully!");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ShowFilteredCars(Guid[] carsId)
        {//show cars that match search criteria
            var userId = _userManager.GetUserId(User);
            CarModel[] cars = _carDataManager.GetCars(carsId.ToList());

            CarViewModel viewModel = new CarViewModel();
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            //if no cars are found
            if (!cars.Any())
            {
                ModelState.AddModelError("NoCarFoundMessage", "No cars were found that match your criteria!");
            }

            return View("Index", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult OpenAddCar(CarViewModel viewModel = null)
        {//open view to add a new car
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

            ModelState.Clear();
            return View("AddCar", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult OpenAddCarProperties()
        {//open view to add new car properties
            return View("AddCarProperties");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddBrand(string Brand)
        {//add new brand
            if (string.IsNullOrWhiteSpace(Brand))
            {
                return View("AddCarProperties");
            }

            BrandModel[] brands = _carDataManager.GetBrands();

            //check if brand already exists
            if (brands.Any(x => string.Equals(x.Brand, Brand, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This brand already exists in the system!");
                return View("AddCarProperties");
            }
            //add to database
            _carDataManager.AddBrand(Brand);
            _notyfService.Success("Brand added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddCarModel(string CarModel)
        {//add new model
            if (string.IsNullOrWhiteSpace(CarModel))
            {
                return View("AddCarProperties");
            }

            //check if model already exists
            CarModelModel[] carModels = _carDataManager.GetCarModels();
            if (carModels.Any(x => string.Equals(x.Model, CarModel, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This model already exists in the system!");
                return View("AddCarProperties");
            }
            //add to database
            _carDataManager.AddCarModel(CarModel);
            _notyfService.Success("Model added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddFuelType(string FuelType)
        {//add new fuel type
            if (string.IsNullOrWhiteSpace(FuelType))
            {
                return View("AddCarProperties");
            }

            //check if fuel type already exists
            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            if (fuelTypes.Any(x => string.Equals(x.FuelType, FuelType, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This fuel type already exists in the system!");
                return View("AddCarProperties");
            }
            //add to database
            _carDataManager.AddFuelType(FuelType);
            _notyfService.Success("Fuel type added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddGearboxType(string Gearbox)
        {//add new gearbox type
            if (string.IsNullOrWhiteSpace(Gearbox))
            {
                return View("AddCarProperties");
            }

            //check if gearbox type already exists
            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            if (gearboxTypes.Any(x => string.Equals(x.Gearbox, Gearbox, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This gearbox type already exists in the system!");
                return View("AddCarProperties");
            }
            //add to database
            _carDataManager.AddGearboxType(Gearbox);
            _notyfService.Success("Gearbox type added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddLocation(string City, string Street, string Number)
        {//add new location
            if (string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(Number))
            {
                return View("AddCarProperties");
            }

            //check if location already exists
            LocationModel[] locations = _carDataManager.GetLocations();
            if (locations.Any(x =>
            string.Equals(x.City, City, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Street, Street, StringComparison.InvariantCultureIgnoreCase) &&
            string.Equals(x.Number, Number, StringComparison.InvariantCultureIgnoreCase)))
            {
                _notyfService.Error("This location already exists in the system!");
                return View("AddCarProperties");
            }
            //add to database
            _carDataManager.AddLocation(City, Street, Number);
            _notyfService.Success("Location added successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ShowCarProperties(CarPropertiesViewModel viewModel = null)
        {//open view with car properties
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

            //if nothing is added, show message
            if (!brands.Any())
            {
                ModelState.AddModelError("NoBrandsAdded", "No brands have been added in the system!");
            }
            if (!models.Any())
            {
                ModelState.AddModelError("NoModelsAdded", "No models have been added in the system!");
            }
            if (!fuelTypes.Any())
            {
                ModelState.AddModelError("NoFuelTypesAdded", "No fuel types have been added in the system!");
            }
            if (!gearboxTypes.Any())
            {
                ModelState.AddModelError("NoGearboxTypesAdded", "No gearbox types have been added in the system!");
            }
            if (!locations.Any())
            {
                ModelState.AddModelError("NoLocationsAdded", "No locations have been added in the system!");
            }

            return View("ShowCarProperties", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditBrand(CarPropertiesViewModel model)
        {//edit selected brand
            if (string.IsNullOrWhiteSpace(model.Brand))
            {
                return RedirectToAction(nameof(ShowCarProperties), model);
            }
            var existingBrand = _carDataManager.GetOneBrand(model.BrandId);

            //check if brand already exists
            BrandModel[] brands = _carDataManager.GetBrands();
            if (brands.Any(x => string.Equals(x.Brand, model.Brand, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingBrand.Brand, model.Brand, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This brand already exists in the system!");
                    return View(model);
                }
            }
            //edit in database
            _carDataManager.EditBrand(model.BrandId, model.Brand);
            _notyfService.Success("Brand edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditBrand(Guid BrandId)
        {//get info for editing
            // get brand from database
            BrandModel brand = _carDataManager.GetOneBrand(BrandId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.BrandId = brand.Id;
            viewModel.Brand = brand.Brand;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditModel(CarPropertiesViewModel model)
        {//edit selected model
            if (string.IsNullOrWhiteSpace(model.CarModel))
            {
                return RedirectToAction(nameof(ShowCarProperties), model);
            }
            var existingModel = _carDataManager.GetOneCarModel(model.CarModelId);

            //check if model already exists
            CarModelModel[] carModels = _carDataManager.GetCarModels();
            if (carModels.Any(x => string.Equals(x.Model, model.CarModel, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingModel.Model, model.CarModel, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This model already exists in the system!");
                    return View(model);
                }
            }
            //edit in database
            _carDataManager.EditCarModel(model.CarModelId, model.CarModel);
            _notyfService.Success("Model edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditModel(Guid CarModelId)
        {//get info for editing
            // get car model from database
            CarModelModel carModel = _carDataManager.GetOneCarModel(CarModelId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.CarModelId = carModel.Id;
            viewModel.CarModel = carModel.Model;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditFuelType(CarPropertiesViewModel model)
        {//edit selected fuel type
            if (string.IsNullOrWhiteSpace(model.FuelType))
            {
                return RedirectToAction(nameof(ShowCarProperties), model);
            }
            var existingFuelType = _carDataManager.GetOneFuelType(model.FuelTypeId);

            //check if fuel type already exists
            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            if (fuelTypes.Any(x => string.Equals(x.FuelType, model.FuelType, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingFuelType.FuelType, model.FuelType, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This fuel type already exists in the system!");
                    return View(model);
                }
            }
            //edit in database
            _carDataManager.EditFuelType(model.FuelTypeId, model.FuelType);
            _notyfService.Success("Fuel type edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditFuelType(Guid FuelTypeId)
        {//get infor for editing
            // get fuel type from database
            FuelTypeModel fuelType = _carDataManager.GetOneFuelType(FuelTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.FuelTypeId = fuelType.Id;
            viewModel.FuelType = fuelType.FuelType;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditGearbox(CarPropertiesViewModel model)
        {//edit selected gearbox type
            if (string.IsNullOrWhiteSpace(model.Gearbox))
            {
                return RedirectToAction(nameof(ShowCarProperties), model);
            }
            var existingGearboxType = _carDataManager.GetOneGearboxType(model.GearboxId);

            //check if gearbox type already exists
            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            if (gearboxTypes.Any(x => string.Equals(x.Gearbox, model.Gearbox, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.Equals(existingGearboxType.Gearbox, model.Gearbox, StringComparison.InvariantCultureIgnoreCase))
                {
                    _notyfService.Error("This gearbox type already exists in the system!");
                    return View(model);
                }
            }
            //edit in databse
            _carDataManager.EditGearboxType(model.GearboxId, model.Gearbox);
            _notyfService.Success("Gearbox type edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditGearbox(Guid GearboxTypeId)
        {//get info for editing
            // get gearbox type from database
            GearboxModel gearbox = _carDataManager.GetOneGearboxType(GearboxTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.GearboxId = gearbox.Id;
            viewModel.Gearbox = gearbox.Gearbox;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditLocation(CarPropertiesViewModel model)
        {//edit selected location
            if (string.IsNullOrWhiteSpace(model.City) || string.IsNullOrWhiteSpace(model.Street) || string.IsNullOrWhiteSpace(model.Number))
            {
                return RedirectToAction(nameof(ShowCarProperties), model);
            }
            var existingLocation = _carDataManager.GetOneLocation(model.LocationId);

            //check if location already exists
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
            //edit in database
            _carDataManager.EditLocation(model.LocationId, model.City, model.Street, model.Number);
            _notyfService.Success("Location edited successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditLocation(Guid LocationId)
        {//get info for editing
            // get location from database
            LocationModel location = _carDataManager.GetOneLocation(LocationId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.LocationId = location.Id;
            viewModel.City = location.City;
            viewModel.Street = location.Street;
            viewModel.Number = location.Number;

            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteBrand(Guid BrandId)
        {//delete selected brand
            CarModel[] cars = _carDataManager.GetCarsByBrand(BrandId);

            //check if any car has the selected brand
            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a brand with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            //delete from database
            _carDataManager.DeleteBrand(BrandId);
            _notyfService.Success("Brand deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteModel(Guid CarModelId)
        {//delete selected model
            CarModel[] cars = _carDataManager.GetCarsByCarModel(CarModelId);

            //check if any car has the selected model
            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a model with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            //delete from database
            _carDataManager.DeleteCarModel(CarModelId);
            _notyfService.Success("Model deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteFuelType(Guid FuelTypeId)
        {//delete selected fuel type
            CarModel[] cars = _carDataManager.GetCarsByFuelType(FuelTypeId);

            //check if any car has the selected fuel type
            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a fuel type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            //delete from database
            _carDataManager.DeleteFuelType(FuelTypeId);
            _notyfService.Success("Fuel type deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteGearbox(Guid GearboxTypeId)
        {//delete selected gearbox type
            CarModel[] cars = _carDataManager.GetCarsByGearboxType(GearboxTypeId);

            //check if any car has the selected gearbox type
            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a gearbox type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            //delete from database
            _carDataManager.DeleteGearboxType(GearboxTypeId);
            _notyfService.Success("Gearbox type deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteLocation(Guid LocationId)
        {//delete selected location
            CarModel[] cars = _carDataManager.GetCarsByLocation(LocationId);

            //check if any car has the selected location
            if (cars.Any())
            {
                _notyfService.Error("Cannot delete a location with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            //delete from database
            _carDataManager.DeleteLocation(LocationId);
            _notyfService.Success("Location deleted successfully!");

            return RedirectToAction(nameof(ShowCarProperties));
        }


        [Authorize(Roles = "RegisteredUser")]
        [HttpPost]
        public IActionResult AddFeedback(Guid CarId, string comment)
        {//add feedback for a car
            //check if input is not empty
            if (string.IsNullOrWhiteSpace(comment))
            {
                _notyfService.Error("Please input your comment!");
                return RedirectToAction(nameof(OpenCar), new { CarId });
            }
            var user = _userManager.GetUserAsync(User).Result;
            //add to database
            _carDataManager.AddFeedback(CarId, comment, user);
            _notyfService.Success("Feedback added successfully!");

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteFeedback(Guid FeedbackId, Guid CarId)
        {//delete selected feedback from database
            _carDataManager.DeleteFeedback(FeedbackId);
            _notyfService.Success("Feedback deleted successfully!");

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }
    }
}
