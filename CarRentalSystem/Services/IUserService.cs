using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<string> AuthenticateUserAsync(string email, string password);
    }
}
