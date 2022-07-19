using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
<<<<<<< HEAD
using RestaurantAPI.Models;
=======
>>>>>>> 3e76f3dc25589f9b5688c72abf0c8ee006420ab2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
<<<<<<< HEAD
    [Route("api/{controller}")]
    public class RestaurantController : ControllerBase
    {
        private RestaurantDbContext _dbContext;
        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDTO>> GetAll()
        {
            return Ok(_dbContext.Restaurants.ToList()) ;
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<RestaurantDTO> Get([FromRoute] int restaurantId)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == restaurantId);



            if (restaurant != null)
                return Ok(restaurant);
            else
                return NotFound();
        }
=======
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
       
>>>>>>> 3e76f3dc25589f9b5688c72abf0c8ee006420ab2
    }
}
