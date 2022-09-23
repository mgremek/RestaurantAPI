using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost]
        public ActionResult CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDTO dish)
            => Created($"api/{restaurantId}/dish/{_dishService.CreateDish(dish, restaurantId)}", null);

        [HttpGet("{dishId}")]
        public ActionResult<DishDTO> GetDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDTO dish = _dishService.GetDish(restaurantId, dishId);
            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDTO>> GetAllDishes([FromRoute] int restaurantId)
        {
            IEnumerable<DishDTO> dishes = _dishService.GetAllDishes(restaurantId);
            return Ok(dishes);
        }

        [HttpDelete("all")]
        public ActionResult DeleteAllDishes([FromRoute] int restaurantId)
        {
            _dishService.DeleteAll(restaurantId);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.DeleteDishById(restaurantId, dishId);
            return NoContent();
        }
    }
}
