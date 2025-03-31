using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkFinder.Application.Helpers
{
    public class Common
    {
        public static bool IsValidInteger(string input)
        {
            int result;

            if (string.IsNullOrWhiteSpace(input))
            {
                result = 0;
                return false; // Input is empty or whitespace
            }

            if (!int.TryParse(input, out result))
            {
                return false; // Not a valid integer
            }

            if (result < 0)
            {
                return false; // Integer is negative
            }

            return true; // Valid integer (0 or greater)
        }
    }
}
