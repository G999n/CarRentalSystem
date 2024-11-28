// CarController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Models;
using CarRentalSystem.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public CarController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    // GET /cars: Get a list of available cars
    [HttpGet]
    public async Task<IActionResult> GetAvailableCars()
    {
        // Get a list of cars that are available for rent
        var availableCars = await _carRepository.GetAvailableCars();
        if (availableCars == null || availableCars.Count == 0)
        {
            return NotFound(new { message = "No available cars found." });
        }

        return Ok(availableCars);
    }

    // POST /cars: Add a new car to the fleet
    [Authorize] // JWT authentication is required to add a car
    [HttpPost]
    public async Task<IActionResult> AddCar([FromBody] Car car)
    {
        if (car == null)
        {
            return BadRequest(new { message = "Car data is required." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns validation errors
        }

        // Add the car to the repository
        await _carRepository.AddCar(car);

        return Ok(new { message = "Car added successfully", car });
    }

    // PUT /cars/{id}: Update car details
    [Authorize] // JWT authentication is required to update car details
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
    {
        if (car == null || id != car.Id)
        {
            return BadRequest("Car ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns validation errors
        }

        // Update the car details
        await _carRepository.UpdateCar(car);

        return Ok(new { message = "Car updated successfully" });
    }

    // DELETE /cars/{id}: Delete a car from the fleet
    [Authorize] // JWT authentication is required to delete a car
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carRepository.DeleteCar(id); // Call DeleteCar method
        return Ok(new { message = "Car deleted successfully" });
    }
}
