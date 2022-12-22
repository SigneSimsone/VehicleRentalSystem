﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace VehicleRentalSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CarDataManager _carDataManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly INotyfService _notyfService;
        public ReservationController(CarDataManager carDataManager, UserManager<UserModel> userManager, INotyfService notyfService)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult Confirmation(ReservationViewModel viewModel)
        {
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

            PaymentModel payment = _carDataManager.GetPayment(viewModel.PaymentId);
            ReservationModel reservation = _carDataManager.GetOneReservation(viewModel.ReservationId);

            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = payment.Amount;
            viewModel.PaymentDateString = payment.Date.ToString();

            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationViewModel model)
        {
            if (model.StartDate < DateTime.Now || model.StartDate > model.EndDate || model.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("IncorrectDates", "Please input correct dates! Dates cannot be in the past. End date has to be larger than start date.");
                return View(nameof(AddReservation), model);
            }

            ReservationModel[] reservations = _carDataManager.CheckIfDatesValid(model.CarId, model.StartDate, model.EndDate);
            if (reservations.Any())
            {
                ModelState.AddModelError("CarNotAvailable", "This car is not available for the selected time period.");
                return View(nameof(AddReservation), model);
            }

            var user = await _userManager.GetUserAsync(User);

            CarModel car = _carDataManager.GetOneCar(model.CarId);

            TimeSpan timeSpan = model.EndDate - model.StartDate;
            double days = timeSpan.TotalDays;
            double amount = car.DailyPrice * days;

            model.PaymentId = _carDataManager.AddPayment(amount);
            PaymentModel payment = _carDataManager.GetPayment(model.PaymentId);

            model.ReservationId = _carDataManager.AddReservation(model.StartDate, model.EndDate, car.Id, user, payment);
            model.Amount = amount;

            model.SuccessMessage = "Reservation successful!";

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


        public IActionResult UserReservationHistory()
        {
            var user = _userManager.GetUserAsync(User).Result;

            ReservationViewModel viewModel = new ReservationViewModel();
            var reservations = _carDataManager.GetUserReservations(user);

            viewModel.Reservations = reservations;
            viewModel.UserId = user.Id;

            return View("UserReservationHistory", viewModel);
        }

        public IActionResult AllReservations()
        {
            var user = _userManager.GetUserAsync(User).Result;

            ReservationViewModel viewModel = new ReservationViewModel();
            var reservations = _carDataManager.GetReservations();
            CarModel[] car = _carDataManager.GetCars();

            viewModel.Reservations = reservations;
            viewModel.UserId = user.Id;
            viewModel.Email = user.Email;

            return View("AllReservations", viewModel);
        }

        [HttpPost]
        public IActionResult AddPaymentDate(DateTime PaymentDate, Guid ReservationId, ReservationViewModel model)
        {
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            _carDataManager.AddPaymentDate(reservation.PaymentId, PaymentDate);
            _notyfService.Success("Payment date added successfully!");

            return RedirectToAction(nameof(AllReservations));
        }

        [HttpGet]
        public IActionResult AddPaymentDate(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(AllReservations));
            }

            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.ReservationId = ReservationId;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Email = reservation.User.Email;

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult OpenReservation(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(UserReservationHistory));
            }

            // get reservation from database
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

            return View(nameof(Confirmation), viewModel);
        }

        [HttpGet]
        public IActionResult OpenReservationAdmin(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(AllReservations));
            }

            // get reservation from database
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

            return View(nameof(Confirmation), viewModel);
        }


        [HttpPost]
        public IActionResult DeleteReservation(Guid ReservationId)
        {
            _carDataManager.DeleteReservation(ReservationId);

            return RedirectToAction(nameof(UserReservationHistory));
        }

        [HttpPost]
        public IActionResult DeleteReservationAdmin(Guid ReservationId)
        {
            _carDataManager.DeleteReservation(ReservationId);

            return RedirectToAction(nameof(AllReservations));
        }
    }
}
