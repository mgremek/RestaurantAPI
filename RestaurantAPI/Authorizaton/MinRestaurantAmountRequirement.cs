using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorizaton
{
    public class MinRestaurantAmountRequirement : IAuthorizationRequirement
    {
        public int MinRestaurantsCreated { get; }

        public MinRestaurantAmountRequirement(int minRestaurantsCreated)
        {
            MinRestaurantsCreated = minRestaurantsCreated;
        }
    }
}
