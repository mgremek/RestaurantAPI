using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street));

            CreateMap<Dish, DishDTO>();

            CreateMap<CreateRestaurantDTO, Restaurant>()
                .ForMember(m => m.Address, 
                c => c.MapFrom(dto => new Address()
                     { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
             
        }
    }
}
