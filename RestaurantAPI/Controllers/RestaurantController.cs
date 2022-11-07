using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            return Created($"/api/restaurant/{_restaurantService.CreateNew(dto)}", null); 
        }

        [HttpGet]
        //[Authorize(Policy = "AtLeast2rest")]
        public ActionResult<IEnumerable<Restaurant>> GetAllRestaurants([FromQuery]RestaurantQuery query) => Ok(_restaurantService.GetAll(query));

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetRestaurantById([FromRoute]int restaurantId)
        {
            var restaurant =_restaurantService.GetById(restaurantId);
            return Ok(restaurant);
        }   
    }
}
