using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Entities;
using System.Net;
using Xunit;

namespace RestaurantApi.IntegrationTests
{
    public class RestaurantControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        
        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = new WebApplicationFactory<Program>();
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("ResturantDb"));
                    });
                })
                .CreateClient();
        }
        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        [InlineData("pageSize=10&pageNumber=3")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string query)
        {

            var response = await _client.GetAsync($"/api/restaurant?{query}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=4&pageNumber=1")]
        [InlineData("pageSize=11&pageNumber=2")]
        public async Task GetAll_WithQueryParameters_ReturnsBadRequest(string query)
        {          
            var response = await _client.GetAsync($"/api/restaurant?{query}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}