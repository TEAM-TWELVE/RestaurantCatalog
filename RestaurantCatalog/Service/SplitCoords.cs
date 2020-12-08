using RestaurantCatalog.Model;
using RestaurantCatalog.Utils;
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
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            

            ICollection<Coordinates> coordinatesList = new List<Coordinates>();        
        
            foreach (var coord in coords)
            {
                var coordSplit = coord.Split(",");                           
                coordinatesList.Add(new Coordinates { Lat = coordSplit[0], Lng = coordSplit[1] });
            }
            return coordinatesList;
        }
    }
}
