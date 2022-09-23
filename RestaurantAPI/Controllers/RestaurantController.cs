using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    { 
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(RestaurantDbContext restaurantDBContext, IMapper mapper, IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpDelete("{restaurantId}")]
        public ActionResult DeleteRestaurant([FromRoute]int restaurantId)
        {
            _restaurantService.Delete(restaurantId);
            return NoContent();
        }

        [HttpPut("{restaurantId}")]
        public ActionResult ModifyRestaurant([FromRoute] int restaurantId, [FromBody] CreateRestaurantDTO dto)
        {
            _restaurantService.Modify(restaurantId, dto);
            return Ok();        
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO dto)
            => Created($"/api/restaurant/{_restaurantService.CreateNew(dto)}", null);
 
        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAllRestaurants() => Ok(_restaurantService.GetAll());

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetRestaurantById([FromRoute]int restaurantId)
        {
            var restaurant =_restaurantService.GetById(restaurantId);
            return Ok(restaurant);
        }   
    }
}
