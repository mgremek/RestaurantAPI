using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApi.IntegrationTests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _webAppFactory;
        private readonly List<Type> _controllerTypes;


        public ProgramTests(WebApplicationFactory<Program> webAppFactory)
        {
            _controllerTypes = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            _webAppFactory = webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }
        [Fact]
        public void ConfigureServices_ForControllers_RegisterAllDependencies()
        {
            var scopeFactory = _webAppFactory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();

            var controller = scope.ServiceProvider.GetService<AccountController>();

            controller.Should().NotBeNull();
            _controllerTypes.ForEach(t =>
            {
                var controller = scope.ServiceProvider.GetService(t);

                controller.Should().NotBeNull();
            });
        }
    }
}
