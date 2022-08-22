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
            return _restaurantService.Delete(restaurantId) ? NoContent() : NotFound();
        }

        [HttpPut("{restaurantId}")]
        public ActionResult ModifyRestaurant([FromRoute] int restaurantId, [FromBody] CreateRestaurantDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_restaurantService.Modify(restaurantId, dto))
                return Ok();
            else
                return NotFound();
        }
        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Created($"/api/restaurant/{_restaurantService.CreateNew(dto)}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAllRestaurants() => Ok(_restaurantService.GetAll());

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetRestaurantById([FromRoute]int restaurantId)
        {
            var restaurant =_restaurantService.GetById(restaurantId);

            if (restaurant is null)
                return NotFound();
            else
                return Ok(restaurant);
        }   
    }
}
