using CarRentalSystem.Models;
using System.Threading.Tasks;


namespace CarRentalSystem.Repositories {
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
    }

}

