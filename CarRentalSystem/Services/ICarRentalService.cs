public interface ICarRentalService
{
    Task<bool> RentCar(int carId, int userId, DateTime rentalDate, int duration);
    Task<bool> CheckCarAvailability(int carId);
}
