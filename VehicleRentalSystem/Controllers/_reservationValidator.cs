using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    internal class _reservationValidator
    {
        private readonly CarDataManager _carDataManager;

        public _reservationValidator(CarDataManager carDataManager)
        {
            _carDataManager = carDataManager;
        }
        internal bool AreDatesValid(Guid carId, DateTime requestedStartDate, DateTime requestedEndDate)
        {
            CarModel[] cars = _carDataManager.CheckIfDatesValid(carId, requestedStartDate, requestedEndDate);
            //Guid[] carIdList = cars.Select(x => x.Id).ToArray();
            if(!cars.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}