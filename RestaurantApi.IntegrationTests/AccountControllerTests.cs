using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantApi.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private Mock<IAccountService> _accServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IAccountService>(_accServiceMock.Object); 

                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("ResturantDb"));
                    });
                });


            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TasRegisterUser_ForValidModel_ReturnsOk()
        {
            var registerUser = new RegisterUserDTO()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(registerUser), Encoding.UTF8, "application/json");

            var res = await _client.PostAsync("/api/account/register", httpContent);

            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task TasRegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            var registerUser = new RegisterUserDTO()
            {
                Email = "testcostam",
                Password = "password123",
                ConfirmPassword = "password"
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(registerUser), Encoding.UTF8, "application/json");

            var res = await _client.PostAsync("/api/account/register", httpContent);

            res.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Login_ForRegisteredUser_ReturnOk()
        {
            _accServiceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "password123"
            };

            var httpContent = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("/api/account/login", httpContent);

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
    }
}

