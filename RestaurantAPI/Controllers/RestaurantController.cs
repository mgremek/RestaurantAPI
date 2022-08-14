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

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Created($"/api/restaurant/{_restaurantService.CreateNew(dto)}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll() => Ok(_restaurantService.GetAll());

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> Get([FromRoute]int restaurantId)
        {
            var restaurant =_restaurantService.GetById(restaurantId);

            if (restaurant is null)
                return NotFound();
            else
                return Ok(restaurant);
        }   
    }
}
