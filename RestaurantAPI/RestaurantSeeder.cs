﻿using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name="KFC",
                    Category = "FastFood",
                    Description="KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky.",
                    ContactEmail="contact@kfc.com",
                    HasDelivery=true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Hot Wings",
                            Price = 10.30M
                        },

                    },
                    Address=new Address()
                    {
                        City="Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Restaurant()
                {
                    Name="McDonald's",
                    Category="FastFood",
                    Description="An American-based multinational fast food chain, founded in 1940 in San Bernardino, California.",
                    ContactEmail="contact@mcdonald.com",
                    HasDelivery=true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "McRoyal",
                            Price = 15.60M
                        },
                        new Dish()
                        {
                            Name="McNuggets",
                            Price=8.20M
                        }

                    },
                    Address=new Address()
                    {
                        City="Warszawa",
                        Street = "Branickiego 25a",
                        PostalCode = "02-972"
                    }
                }
            };

            return restaurants;
        }
    }
}

