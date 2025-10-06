using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using TesteandoMVC.Web.Services;
using Xunit;

namespace TesteandoMVC.Tests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<TesteandoMVC.Web.Program>>
    {
        private readonly WebApplicationFactory<TesteandoMVC.Web.Program> _factory;

        public HomeControllerTests(WebApplicationFactory<TesteandoMVC.Web.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_CuandoElServicioDevuelveTrue_MuestraMensajeDeExito()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.HoraEsPar()).Returns(true);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡El servicio funciona correctamente!", content);
        }

        [Fact]
        public async Task Index_CuandoElServicioDevuelveFalse_MuestraMensajeDeError()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.HoraEsPar()).Returns(false);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡El servicio no está funcionando!", content);
        }
    }
}
