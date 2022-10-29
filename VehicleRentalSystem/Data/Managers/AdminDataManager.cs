using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data.Managers
{
    public class AdminDataManager
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminDataManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserModel[] GetUsers()
        {
            var result = _dbContext
                .Users
                .ToArray();

            return result;
        }
        public UserModel GetOneUserByEmail(string email)
        {
            var item = _dbContext.Users.First(x => x.Email == email);

            return item;
        }

        public UserModel GetOneUserById(string id)
        {
            var item = _dbContext.Users.First(x => x.Id == id);

            return item;
        }
    }
}
