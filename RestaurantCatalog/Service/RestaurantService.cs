using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestaurantCatalog.Interfaces;
using RestaurantCatalog.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace RestaurantCatalog.Service
{
    public class RestaurantService : IRestaurantService
    {
        private Logger _logger = new Logger("RestaurantService");
        public IConfiguration Configuration { get; private set; }
        public RestaurantService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<ICollection<Restaurant>> GetRestaurants(Coordinates coords)
        {
            _logger.Info("GetRestaurants() called.");
            ICollection<Restaurant> restaurants = new List<Restaurant>();
            string apiKey = Configuration["ExternalProvides:Google:APIKey"];

            using (HttpClient client = new HttpClient())
            {
                _logger.Info("Successfully entered using block with HttpClient initialization.");
                var response = await client.GetAsync(Configuration["APIEndpoints:Google:Places:RestaurantDiscoverer"] + coords.Lat + "," + coords.Lng + "&radius=2000&type=restaurant&key=" + apiKey);
                if (response.IsSuccessStatusCode)
                {
                    _logger.Info("Status code was " + response.StatusCode);
                    var content = await response.Content.ReadAsStringAsync();
                    restaurants = GetRestaurantFromJson(content);
                }
                else
                {
                    _logger.Error("Couldn't get response with coordinates: " + coords.Lat + " " + coords.Lng);
                    throw new HttpRequestException("Couldn't get response... StatusCode: " + response.StatusCode);
                }
            }
            _logger.Info("Exiting GetRestaurants()");
            return restaurants;
        }
        private ICollection<Restaurant> GetRestaurantFromJson(string json)
        {
            _logger.Info("GetRestaurantFromJson() called.");
            ICollection<Restaurant> listToReturn = new List<Restaurant>();
            JObject restaurants = JsonConvert.DeserializeObject<dynamic>(json);
            _logger.Info("Deserialized json into JObject.");
            var results = restaurants.SelectToken("results").ToList();
            for (int i = 0; i < results.Count && i < 3; i++)
            {
                _logger.Info("Inside fori-loop with index: " + i);
                if (null != results[i])
                {
                    _logger.Info("JToken on index " + i + "was NOT null.");
                    JObject restaurant = JsonConvert.DeserializeObject<dynamic>(results[i].ToString());
                    var address = (string)restaurant.SelectToken("vicinity");
                    var name = (string)restaurant.SelectToken("name");
                    var rating = (double)restaurant.SelectToken("rating");
                    _logger.Info("Read all values from JToken into local variables.");
                    listToReturn.Add(new Restaurant { Address = address, Name = name, Rating = rating });
                    _logger.Info("Added new Restaurant instance to List<Restaurant>.");
                }

            }
            _logger.Info("Exiting GetRestaurantFromJson()");
            return listToReturn;
        }
        
    }
}
