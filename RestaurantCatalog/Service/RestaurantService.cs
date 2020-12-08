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
        public IConfiguration Configuration { get; private set; }
        public RestaurantService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<ICollection<Restaurant>> GetRestaurants(ICollection<Coordinates> coords)
        {
            ICollection<Restaurant> restaurants = new List<Restaurant>();
            string apiKey = Configuration["ExternalProvides:Google:APIKey"];

            foreach(var coord in coords)
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(Configuration["APIEndpoints:Google:Places:RestaurantDiscoverer"] + coord.Lat + "," + coord.Lng + "&radius=2000&type=restaurant&key=" + apiKey);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        restaurants = GetRestaurantFromJson(content);
                    }
                    else
                    {
                        throw new HttpRequestException("Couldn't get response... StatusCode: " + response.StatusCode);
                    }
                }
            }
            return restaurants;
        }
        private ICollection<Restaurant> GetRestaurantFromJson(string json)
        {
            ICollection<Restaurant> listToReturn = new List<Restaurant>();
            JObject restaurants = JsonConvert.DeserializeObject<dynamic>(json);
            var results = restaurants.SelectToken("results").ToList();
            for (int i = 0; i < 3; i++)
            {
                if (results[i] != null || results[i].HasValues)
                {
                    JObject restaurant = JsonConvert.DeserializeObject<dynamic>(results[i].ToString());
                    var address = (string)restaurant.SelectToken("vicinity");
                    var name = (string)restaurant.SelectToken("name");
                    var rating = (double)restaurant.SelectToken("rating");
                    listToReturn.Add(new Restaurant { Address = address, Name = name, Rating = rating });
                }
            }
            return listToReturn;
        }
        
    }
}
