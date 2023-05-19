using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace RestaurantApi.IntegrationTests
{
    public class RestaurantControllerTests
    {
        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        [InlineData("pageSize=10&pageNumber=3")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string query)
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync($"/api/restaurant?{query}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Theory]
        [InlineData("pageSize=4&pageNumber=1")]
        [InlineData("pageSize=11&pageNumber=2")]
        public async Task GetAll_WithQueryParameters_ReturnsBadRequest(string query)
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync($"/api/restaurant?{query}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
    }
}