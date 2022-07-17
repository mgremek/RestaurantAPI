using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        RestaurantDbContext _restaurantDbContext;
        public RestaurantController(RestaurantDbContext restaurantDBContext)
        {
            _restaurantDbContext = restaurantDBContext;
        }
        public ActionResult<IEnumerable<Restaurant>> GetAll()
            => Ok(_restaurantDbContext.Restaurants.ToList());

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> Get([FromRoute]int restaurantId)
        {
            var restaurant = _restaurantDbContext.Restaurants.FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant is null)
                return NotFound();

            return Ok(restaurant);
        }
       
    }
}
