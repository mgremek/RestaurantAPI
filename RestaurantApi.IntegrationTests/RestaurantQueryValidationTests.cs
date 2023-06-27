using FluentValidation.TestHelper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApi.IntegrationTests
{
    public  class RestaurantQueryValidationTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery()
                {
                    PageNumber=1,
                    PageSize=5,
                    SortBy=nameof(Restaurant.Name)
                },
                new RestaurantQuery()
                {
                    PageNumber=8,
                    PageSize=10,
                    SortBy=nameof(Restaurant.Name)
                },
                new RestaurantQuery()
                {
                    PageNumber=1,
                    PageSize=15,
                    SortBy=nameof(Restaurant.Name)
                },
                new RestaurantQuery()
                {
                    PageNumber=12,
                    PageSize=15,
                    SortBy=nameof(Restaurant.Category)
                },
            };

            return list.Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> GetSampleInvalidData() 
        {
            var list = new List<RestaurantQuery>
            {
                new RestaurantQuery()
                {
                    PageNumber=1,
                    PageSize=1,
                },
                new RestaurantQuery()
                {
                    PageNumber=0,
                    PageSize=10,
                },
                new RestaurantQuery()
                {
                    PageNumber=1,
                    PageSize=15,
                    SortBy=nameof(Restaurant.Address)
                },
                new RestaurantQuery()
                {
                    PageNumber=-1,
                    PageSize=11,
                    SortBy=nameof(Restaurant.HasDelivery)
                },
            };

            return list.Select(_ => new object[] { _ });
        }
        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSuccess(RestaurantQuery model)
        {
            var validator = new RestaurantQueryValidator();          

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors(); 
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnFailure(RestaurantQuery model)
        {
            var validator = new RestaurantQueryValidator();

            var result = validator.TestValidate(model);

            result.ShouldHaveAnyValidationError();
        }
    }
}
