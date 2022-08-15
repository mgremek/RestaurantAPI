using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        public RestaurantDTO GetById(int id);
        public IEnumerable<RestaurantDTO> GetAll();
        public int CreateNew(CreateRestaurantDTO createRest);
        bool Delete(int id);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private IMapper _mapper;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public RestaurantDTO GetById(int id)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .FirstOrDefault(x => x.Id == id);

            if (restaurant is null) 
                return null;
            else
                return _mapper.Map<RestaurantDTO>(restaurant);
        }

        public IEnumerable<RestaurantDTO> GetAll()
        {
            return _mapper.Map<List<RestaurantDTO>>(
                _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList());
        }
        public int CreateNew(CreateRestaurantDTO createRest)
        {
            var restaurant = _mapper.Map<Restaurant>(createRest);

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public bool Delete(int id)
        {
            var res = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (res is null)
                return false;

            _dbContext.Restaurants.Remove(res);
            _dbContext.SaveChanges();
            return true;
        }
     
    }
}
