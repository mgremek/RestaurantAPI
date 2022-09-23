using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
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
        void Delete(int id);
        void Modify(int id, CreateRestaurantDTO dto);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public RestaurantDTO GetById(int id)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

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

        public void Delete(int id)
        {
            _logger.LogInformation($"Restaurant with id: {id} DELETE action invoked.");

            var res = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (res is null)
                throw new NotFoundException("Restaurant not found");

            _dbContext.Restaurants.Remove(res);
            _dbContext.SaveChanges();
        }

        public void Modify(int id, CreateRestaurantDTO dto)
        {
            var res = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (res is null)
                throw new NotFoundException("Restaurant not found");

            res.Name = dto.Name is null ? res.Name : dto.Name;
            res.Description = dto.Description is null ? res.Description : dto.Description;
            res.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
