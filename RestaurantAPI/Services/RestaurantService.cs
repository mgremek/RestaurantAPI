﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorizaton;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        public RestaurantDTO GetById(int id);
        public PagedResult<RestaurantDTO> GetAll(RestaurantQuery query);
        public int CreateNew(CreateRestaurantDTO createRest);
        void Delete(int id);
        void Modify(int id, CreateRestaurantDTO dto);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, 
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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

        public PagedResult<RestaurantDTO> GetAll(RestaurantQuery query)
        {
            var restaurantsQuery = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhase == null || (r.Name.ToLower().Contains(query.SearchPhase.ToLower())
                                                  || r.Description.ToLower().Contains(query.SearchPhase.ToLower())));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category }
                };

                restaurantsQuery = query.SortDirection == SortDirection.ASC
                   ? restaurantsQuery.OrderBy(columnsSelector[query.SortBy])
                    : restaurantsQuery.OrderByDescending(r => r.Name);
            }

            var pagedRestaurantsQuery = restaurantsQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = _mapper.Map<List<RestaurantDTO>>(pagedRestaurantsQuery);

            return new PagedResult<RestaurantDTO>(result, result.Count, query.PageSize, query.PageNumber);
        }
        public int CreateNew(CreateRestaurantDTO createRest)
        {
            var restaurant = _mapper.Map<Restaurant>(createRest);
            restaurant.CreatedById = _userContextService.GetUserId;

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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, res,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            _dbContext.Restaurants.Remove(res);
            _dbContext.SaveChanges();
        }

        public void Modify(int id, CreateRestaurantDTO dto)
        {
           
            var res = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (res is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, res,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
                throw new ForbidException();

            res.Name = dto.Name is null ? res.Name : dto.Name;
            res.Description = dto.Description is null ? res.Description : dto.Description;
            res.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
