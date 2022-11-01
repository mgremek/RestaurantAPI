using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThan(0);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if(!allowedPageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
            });
        }
    }
}
