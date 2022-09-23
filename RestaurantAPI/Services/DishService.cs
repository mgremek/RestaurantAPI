using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int CreateDish(CreateDishDTO dishDTO, int restaurantId);
        DishDTO GetDish(int restaurantId, int dishId);

        IEnumerable<DishDTO> GetAllDishes(int restaurantId);
        void DeleteAll(int restaurantId);
        void DeleteDishById(int restaurantId, int dishId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _autoMapper;

        public DishService(RestaurantDbContext dbContext, IMapper autoMapper)
        {
            _dbContext = dbContext;
            _autoMapper = autoMapper;
        }
        public int CreateDish(CreateDishDTO dishDTO, int restaurantId)
        {
            var restaurant =_dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            Dish dish = _autoMapper.Map<Dish>(dishDTO);
            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public DishDTO GetDish(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
                throw new NotFoundException("Dish not found");

            return _autoMapper.Map<DishDTO>(dish);
        }

        public IEnumerable<DishDTO> GetAllDishes(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            return _autoMapper.Map<List<DishDTO>>(restaurant.Dishes);
        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.Dishes.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        public void DeleteDishById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Dishes)
               .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }        
    }
}
