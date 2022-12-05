using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CarDataManager _carDataManager;
        private readonly UserManager<UserModel> _userManager;

        public ReservationController(CarDataManager carDataManager, UserManager<UserModel> userManager)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Confirmation(ReservationViewModel viewModel)
        {
            //CarModel car = _carDataManager.GetOneCar(CarId);
            var userId = _userManager.GetUserId(User);

            if (viewModel == null)
            {
                viewModel = new ReservationViewModel();
            }

            if (viewModel.CarId == Guid.Empty)
            {
                return RedirectToAction(nameof(AddReservation), viewModel);
            }

            CarModel car = _carDataManager.GetOneCar(viewModel.CarId);
            viewModel.CarId = car.Id;

            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;

            PaymentModel payment = _carDataManager.GetPayment(viewModel.CarId);
            ReservationModel reservation = _carDataManager.GetOneReservation(viewModel.CarId);

            viewModel.StartDate = reservation.StartDate;
            viewModel.EndDate = reservation.EndDate;
            viewModel.Amount = payment.Amount;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {//vai direkto uz pareizo tagad?
                return RedirectToAction(nameof(AddReservation), model);
            }

            //if(!_reservationValidator.AreDatesValid(model.CarId, model.StartDate, model.EndDate)) //logika
            //{
            //    ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
            //    return View(nameof(AddReservation), model);
            //}

            var user = await _userManager.GetUserAsync(User);

            CarModel carModel = _carDataManager.GetOneCar(model.CarId);

            TimeSpan timeSpan = model.EndDate - model.StartDate;
            double days = timeSpan.TotalDays;
            double amount = model.Car.DailyPrice * days;

            _carDataManager.AddPayment(amount, model.CarId);
            PaymentModel payment = _carDataManager.GetPayment(model.CarId);

            _carDataManager.AddReservation(model.StartDate, model.EndDate, model.CarId, user, payment);
            ReservationModel reservation = _carDataManager.GetOneReservation(model.CarId);

            //no carmodel atlasa info par brand utt
            //padod ConfirmationModel?
            return RedirectToAction(nameof(Confirmation), model);
        }

        [HttpGet]
        public IActionResult AddReservation(Guid carId)
        {
            // get car from database (CarModel)
            CarModel car = _carDataManager.GetOneCar(carId);

            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.CarId = car.Id;

            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;

            return View(viewModel);
        }
    }
}
