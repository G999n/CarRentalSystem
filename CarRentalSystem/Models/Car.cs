using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Make is required")]
        [StringLength(50, ErrorMessage = "Make can't be longer than 50 characters.")]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model can't be longer than 50 characters.")]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price per day must be a positive number")]
        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public bool IsAvailable { get; set; }  // Ensure car availability is tracked

    }
}
