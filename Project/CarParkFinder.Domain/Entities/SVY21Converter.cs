public static class SVY21Converter
{
    private const double a = 6378137;
    private const double f = 1 / 298.257223563;
    private const double oLat = 1.366666; // Latitude of false origin
    private const double oLon = 103.833333; // Longitude of false origin
    private const double N0 = 38744.572; // False Northing
    private const double E0 = 28001.642; // False Easting
    private const double k0 = 1; // Scale factor

    private static readonly double b = a * (1 - f);
    private static readonly double e2 = (2 * f) - (f * f);
    private static readonly double e4 = e2 * e2;
    private static readonly double e6 = e4 * e2;

    public static (double latitude, double longitude) ConvertSVY21ToWGS84(double northing, double easting)
    {
        double n = (a - b) / (a + b);
        double A0 = a * (1 - e2) * (1 + (3.0 / 4.0) * e2 + (45.0 / 64.0) * e4 + (175.0 / 256.0) * e6);
        double B0 = (3.0 / 2.0) * n - (27.0 / 32.0) * Math.Pow(n, 3);
        double C0 = (21.0 / 16.0) * Math.Pow(n, 2) - (55.0 / 32.0) * Math.Pow(n, 4);
        double D0 = (151.0 / 96.0) * Math.Pow(n, 3);
        double E0 = (1097.0 / 512.0) * Math.Pow(n, 4);

        double Np = (northing - N0) / k0;
        double Mo = Np / A0;

        double latRad = Mo + B0 * Math.Sin(2 * Mo) + C0 * Math.Sin(4 * Mo) + D0 * Math.Sin(6 * Mo) + E0 * Math.Sin(8 * Mo);
        double sinLat = Math.Sin(latRad);
        double rho = a * (1 - e2) / Math.Pow(1 - e2 * sinLat * sinLat, 1.5);
        double nu = a / Math.Sqrt(1 - e2 * sinLat * sinLat);

        double t = Math.Tan(latRad);
        double t2 = t * t;
        double l = (easting - E0) / (k0 * nu);
        double l2 = l * l;
        double l4 = l2 * l2;

        double dLat = (t / (2 * rho * nu)) * (l2 - (5 + 3 * t2 + 10 * (e2 - e4) * t2 - 4 * Math.Pow(e2 - e4, 2) - 9 * e4) * l4 / 24);
        double latitude = latRad - dLat;

        double longitude = oLon + (l / Math.Cos(latRad)) - ((1 + 2 * t2 + (e2 - e4)) * l2 * l / (6 * nu * nu)) + ((5 + 28 * t2 + 24 * Math.Pow(t, 4)) * l4 * l / (120 * nu * nu * nu));

        return (latitude * (180 / Math.PI), longitude); // Convert radians to degrees
    }
}
