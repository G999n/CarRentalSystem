public class CarRentalService : ICarRentalService
{
    private readonly ICarRepository _carRepository;

    public CarRentalService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<bool> RentCar(int carId, int userId, DateTime rentalDate, int duration)
    {
        var car = await _carRepository.GetCarById(carId);
        if (car == null || !car.IsAvailable) return false;

        car.IsAvailable = false;
        await _carRepository.UpdateCarAvailability(carId, false);
        return true;
    }

    public async Task<bool> CheckCarAvailability(int carId)
    {
        var car = await _carRepository.GetCarById(carId);
        return car?.IsAvailable ?? false;
    }
}
