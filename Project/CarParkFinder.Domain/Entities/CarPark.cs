using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkFinder.Domain.Entities
{
    public class CarPark
    {
        [Key]
        public required string car_park_no { get; set; }
        public required string address { get; set; }
        public required string x_coord { get; set; }
        public required string y_coord { get; set; }
        public required string car_park_type { get; set; }
        public required string type_of_parking_system { get; set; }
        public required string short_term_parking { get; set; }
        public required string free_parking { get; set; }
        public required string night_parking { get; set; }
        public required string car_park_decks { get; set; }
        public required string gantry_height { get; set; }
        public required string car_park_basement { get; set; }
    }
}
