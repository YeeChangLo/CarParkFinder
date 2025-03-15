using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarParkFinder.Domain.Entities
{
    public class CarParkResponseDto
    {
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int total_lots { get; set; }
        public int available_lots { get; set; }

        [JsonIgnore]
        public double Distance { get; set; } // Calculated distance
    }

}
