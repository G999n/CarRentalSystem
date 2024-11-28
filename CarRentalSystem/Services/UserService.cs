using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalSystem.Models;
using CarRentalSystem.Repositories;

namespace CarRentalSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Register a new user
        public async Task<User> RegisterUserAsync(User user)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            // Hash password before saving to the database (implement hashing in UserRepository or here)
            user.Password = HashPassword(user.Password); // Add your password hashing logic here

            // Add user to the database
            await _userRepository.AddUser(user);
            return user;
        }

        // Authenticate a user and generate a JWT token
        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            // Retrieve user from the repository
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new Exception("Invalid credentials.");
            }

            // Generate JWT token
            return GenerateJwtToken(user);
        }

        // Helper method to generate JWT token
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)  // Assumes the user has a role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expires in 1 hour
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Helper method to hash password (use a strong algorithm like bcrypt in production)
        private string HashPassword(string password)
        {
            // Simple example using SHA256, replace with a better hashing mechanism for production
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Helper method to verify password (compare hashed passwords)
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Compare entered password hash with stored hash
            return storedPasswordHash == HashPassword(enteredPassword);
        }
    }
}
