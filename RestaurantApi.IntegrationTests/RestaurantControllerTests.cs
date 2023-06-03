using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Net;
using System.Text;
using Xunit;

namespace RestaurantApi.IntegrationTests
{
    public class RestaurantControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        public class TestData
        {
            public object Data { get; set; } 
            public HttpStatusCode Result { get; set; }
        }
        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[]
            {
                new TestData() 
                { Data =  new CreateRestaurantDTO() {Name="test", City="Warszawa", Street="testowa 5" }, Result =HttpStatusCode.Created }
            };
            yield return new object[]
            {
                new TestData()
                { Data =  new CreateRestaurantDTO() { }, Result =HttpStatusCode.BadRequest }
            };
        }
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

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("ResturantDb"));
                    });
                })
                .CreateClient();
        }


        //[Fact]
        //public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStatus()
        //{
        //    var model = new CreateRestaurantDTO() { Name = "Test", City = "Warszawa", Street= "Testowa 5" };

        //    var httpContent = new StringContent(JsonConvert.SerializeObject(model), UnicodeEncoding.UTF8, "application/json");

        //    var resp = await _client.PostAsync("/api/restaurant", httpContent);

        //    resp.StatusCode.Should().Be(HttpStatusCode.Created);
        //    resp.Headers.Location.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequest()
        //{
        //    var model = new CreateRestaurantDTO { Name = "Test", City = "Warszawa" };

        //    var httpContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        //    var resp = await _client.PostAsync("/api/restaurant", httpContent);

        //    resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        //}

        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task CreateRestaurant_WithGivenModel_ReturnsStatusRequest(TestData testData)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(testData.Data), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("/api/restaurant", httpContent);

            resp.StatusCode.Should().Be(testData.Result);
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