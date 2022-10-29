using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data.Managers
{
    public class CarDataManager
    {
        private readonly ApplicationDbContext _dbContext;

        public CarDataManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

   /*     public CarModel[] GetCars()
        {
            var result = _dbContext
                .Artists
                .Include(x => x.Users)
                .ToArray();

            return result;
        }*/
    }
}
