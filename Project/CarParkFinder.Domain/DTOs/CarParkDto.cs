using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarParkFinder.Domain.DTOs
{
    public class CarParkDto
    {
        [Key]
        public required string car_park_no { get; set; }
        public string? address { get; set; } = string.Empty;
        public string? x_coord { get; set; } = string.Empty;
        public string? y_coord { get; set; } = string.Empty;
        public string? car_park_type { get; set; } = string.Empty;
        public string? type_of_parking_system { get; set; } = string.Empty;
        public string? short_term_parking { get; set; } = string.Empty;
        public string? free_parking { get; set; } = string.Empty;
        public string? night_parking { get; set; } = string.Empty;
        public string? car_park_decks { get; set; } = string.Empty;
        public string? gantry_height { get; set; } = string.Empty;
        public string? car_park_basement { get; set; } = string.Empty;
    }
}
