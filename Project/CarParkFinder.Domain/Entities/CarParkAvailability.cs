using CarParkFinder.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class CarParkAvailability
{
    [Key]
    public int Id { get; set; } // Auto-incremented Primary Key
    [ForeignKey("CarPark")]
    public required string car_park_no { get; set; } // Foreign Key linking to CarPark
    public required string lot_type { get; set; }
    public required int total_lots { get; set; }
    public required int lots_available { get; set; }
    public required DateTime update_at { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public CarPark? CarPark { get; set; }

}
