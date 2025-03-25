using CarParkFinder.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class CarParkAvailability
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("CarPark")]
    public string car_park_no { get; set; } = string.Empty;
    public string lot_type { get; set; } = string.Empty;
    public int total_lots { get; set; }
    public int lots_available { get; set; }
    public DateTime update_at { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public CarPark? CarPark { get; set; }

}
