using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkFinder.Application.Helpers
{
    public class DistanceHelper
    {
        private const double EarthRadiusKm = 6371.0; // Earth's radius in km

        public static double CalculateDistance(double lat1, double lon1, double y_coord, double x_coord, bool isSVY21)
        {
            double lat2, lon2;

            // Convert SVY21 to WGS84 if needed
            if (isSVY21)
            {
                (lat2, lon2) = SVY21Converter.ConvertSVY21ToWGS84(y_coord, x_coord);
            }
            else
            {
                lat2 = y_coord;
                lon2 = x_coord;
            }

            double R = 6371; // Radius of the Earth in km
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in km
        }
    }
}
