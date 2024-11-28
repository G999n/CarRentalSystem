// CarRepository.cs
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CarRepository : ICarRepository
{
    private readonly CarRentalContext _context;

    public CarRepository(CarRentalContext context)
    {
        _context = context;
    }

    public async Task AddCar(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task<Car> GetCarById(int id)
    {
        return await _context.Cars.FindAsync(id);
    }

    public async Task<List<Car>> GetAvailableCars()
    {
        return await _context.Cars.Where(c => c.IsAvailable).ToListAsync();
    }

    public async Task<List<Car>> GetCars()
    {
        return await _context.Cars.ToListAsync();
    }

    public async Task UpdateCarAvailability(int carId, bool isAvailable)
    {
        var car = await GetCarById(carId);
        if (car != null)
        {
            car.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCar(Car car)
    {
        var existingCar = await _context.Cars.FindAsync(car.Id);
        if (existingCar != null)
        {
            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.PricePerDay = car.PricePerDay;
            existingCar.IsAvailable = car.IsAvailable;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCar(int carId)
    {
        var car = await GetCarById(carId);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
    }
}
