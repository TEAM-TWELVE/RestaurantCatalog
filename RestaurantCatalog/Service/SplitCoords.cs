using RestaurantCatalog.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantCatalog.Service
{
    public class SplitCoords
    {
        public static ICollection<Coordinates> SplitCoordinates(ICollection<string> coords)
        {
            ICollection<Coordinates> coordinatesList = new List<Coordinates>();
            double lat;
            double lng;
            foreach (var coord in coords)
            {
                var coordSplit = coord.Split(",");
                double.TryParse(coordSplit[0], NumberStyles.Any, CultureInfo.CurrentCulture, out lat);
                Console.WriteLine("latitude: " + lat);
                double.TryParse(coordSplit[1], NumberStyles.Any, CultureInfo.CurrentCulture, out lng);
                Console.WriteLine("longitude: " + lng);
                coordinatesList.Add(new Coordinates { Lat = lat, Lng = lng });
            }
            return coordinatesList;
        }
    }
}
