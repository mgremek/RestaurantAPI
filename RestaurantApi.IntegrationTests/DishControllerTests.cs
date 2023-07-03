using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Text;
namespace RestaurantApi.IntegrationTests
{
    public class DishControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly Restaurant _restaurant =  new Restaurant()
        {
            CreatedById = 2,
                Name = "Test",
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "TestDish",
                        Price = 1
                    },
                    new Dish()
                    {
                        Name="TestDish2",
                        Price=12
                    }
                }
        };

        public DishControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       var dbContextOptions = services
                           .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                       services.Remove(dbContextOptions);

                       services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                       services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));

                       services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("ResturantDb"));
                   });
               });


            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllDishes_WithQueryParameters_ReturnsOkResult()
        {
            SeedRestaurant(_restaurant);

            var resp = await _client.GetAsync($"api/restaurant/{_restaurant.Id}/dish");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllDishes_WithQueryParameters_ReturnsNotFound()
        {      
            var resp = await _client.GetAsync($"api/restaurant/-1/dish");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetDish_WithValidParameters_ReturnsOkResult()
        {
            SeedRestaurant(_restaurant);

            var resp = await _client.GetAsync($"api/restaurant/{_restaurant.Id}/dish/{_restaurant.Dishes[0].Id}");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDish_WithInvalidParameters_ReturnsNotFound()
        {
            SeedRestaurant(_restaurant); 

            var resp = await _client.GetAsync($"api/restaurant/{_restaurant.Id}/dish/{100}");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        private void SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task DeleteDish_WithValidParameters_ReturnsNoContent()
        {
            SeedRestaurant(_restaurant);

            var resp = await _client.DeleteAsync($"api/restaurant/{_restaurant.Id}/dish/{_restaurant.Dishes[0].Id}");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteDish_WithInvalidParameters_ReturnsnotFound()
        {
            SeedRestaurant(_restaurant);

            var resp = await _client.DeleteAsync($"api/restaurant/{_restaurant.Id}/dish/{100}");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAllDishes_WithValidParameters_ReturnsNoContent()
        {
            SeedRestaurant(_restaurant);

            var resp = await _client.DeleteAsync($"api/restaurant/{_restaurant.Id}/dish/all");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAllDishes_WithInvalidParameters_ReturnsNoContent()
        {
            var resp = await _client.DeleteAsync($"api/restaurant/{-100}/dish/all");

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        

        [Fact]
        public async Task CreateDish_WithInvalidModel_ReturnsNotFound()
        {
            var dish = new DishDTO()
            {
                Description = "margherita",
                Name = "Pizza",
                Price = 36
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(dish), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/{100}/dish", httpContent);

            resp.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
