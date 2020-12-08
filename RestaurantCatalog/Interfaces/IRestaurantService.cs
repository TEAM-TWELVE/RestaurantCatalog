using RestaurantCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantCatalog.Interfaces
{
    public interface IRestaurantService
    {
        Task<ICollection<Restaurant>> GetRestaurants(ICollection<Coordinates> coords);
    }
}
