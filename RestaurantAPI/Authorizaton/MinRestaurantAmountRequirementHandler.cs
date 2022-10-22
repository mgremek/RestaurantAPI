using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorizaton
{
    public class MinRestaurantAmountRequirementHandler : AuthorizationHandler<MinRestaurantAmountRequirement>
    {
        private readonly RestaurantDbContext _context;

        public MinRestaurantAmountRequirementHandler(RestaurantDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinRestaurantAmountRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            if (_context.Restaurants.ToList().Count(r => r.CreatedById == userId) >= requirement.MinRestaurantsCreated)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
