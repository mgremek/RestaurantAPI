using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;

        public RestaurantController(RestaurantDbContext restaurantDBContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDBContext;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            return Ok(_mapper.Map<List<RestaurantDTO>>(
                _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList()));
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> Get([FromRoute]int restaurantId)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(r=>r.Address)
                .Include(r=>r.Dishes)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant is null)
                return NotFound();

            return Ok(_mapper.Map<RestaurantDTO>(restaurant));
        }
       
    }
}
