using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestaurantCatalog.Interfaces;
using RestaurantCatalog.Model;
using RestaurantCatalog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RestaurantCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private Logger _logger = new Logger("RestaurantController");
        public IConfiguration Configuration { get; private set; }
        public IRestaurantService RestaurantService { get; private set; }
       

        public RestaurantController(IConfiguration configuration, IRestaurantService restaurantService)
        {
            Configuration = configuration;
            RestaurantService = restaurantService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Coordinates coordinates)
        {
            _logger.Info("/ GET Get called.");
            //var coords = SplitCoords.SplitCoordinates(coordinates);
            var result = await RestaurantService.GetRestaurants(coordinates);
            _logger.Info("/ GET successfully finished.");
            return Ok(result);
        }
    }
}
