using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Models.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApi.IntegrationTests
{
    public class RegisterUserDtoValidatorTests
    {
        private readonly RestaurantDbContext _dbContext;

        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<RestaurantDbContext>();
            builder.UseInMemoryDatabase("TestDb");

            _dbContext = new RestaurantDbContext(builder.Options);
            Seed();
        }

        public void Seed()
        {
            var testUsers = new List<User>()
            {
                new User() { Email="test2@test.com"},
                new User() { Email="test3@test.com"}
            };

            _dbContext.Users.AddRange(testUsers);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Validate_ForValidModel_ReturnSuccess()
        {
            var model = new RegisterUserDTO()
            {
                Email = "test@test.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            
            var validator = new RegisterUserDtoValidator(_dbContext);

            var res = validator.TestValidate(model);

            res.ShouldNotHaveAnyValidationErrors();
        }


        [Fact]
        public void Validate_ForInvalidModel_ReturnSuccess()
        {
            var model = new RegisterUserDTO()
            {
                Email = "test2@test.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            var validator = new RegisterUserDtoValidator(_dbContext);

            var res = validator.TestValidate(model);

            res.ShouldHaveAnyValidationError();
        }
    }
}
