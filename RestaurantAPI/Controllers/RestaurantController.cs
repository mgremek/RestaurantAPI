using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/{controller}")]
    public class RestaurantController : ControllerBase
    {
        private RestaurantDbContext _dbContext;
        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            return Ok(_dbContext.Restaurants.ToList()) ;
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> Get([FromRoute] int restaurantId)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant != null)
                return Ok(restaurant);
            else
                return NotFound();
        }
    }
}
