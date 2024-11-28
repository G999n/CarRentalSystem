// ICarRepository.cs
using CarRentalSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICarRepository
{
    Task AddCar(Car car);
    Task<Car> GetCarById(int id);
    Task<List<Car>> GetAvailableCars(); // This method retrieves available cars
    Task<List<Car>> GetCars(); // This method retrieves all cars
    Task UpdateCar(Car car);
    Task UpdateCarAvailability(int carId, bool isAvailable);
    Task DeleteCar(int carId);
}
