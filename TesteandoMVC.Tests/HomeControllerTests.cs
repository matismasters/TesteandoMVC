using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
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
            
            // EJEMPLO: Si el método recibiera parámetros, el mock se haría así:
            // mockService.Setup(x => x.ValidarUsuario("pepitro", "juansito")).Returns(true);
            // mockService.Setup(x => x.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            // mockService.Setup(x => x.ValidarUsuario(It.Is<string>(u => u == "don_correcto"), It.Is<string>(p => p == "iatusabes"))).Returns(true);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/");
            // Alternativamente, si el método fuera POST
            // var response = await client.PostAsync("/", null);
            // O si el método fuera POST con parámetros
            // Para APIs que esperan JSON
            // string json = "{\"usuario\":\"don_correcto\",\"password\":\"iatusabes\"}";
            // StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // HttpResponseMessage response = await client.PostAsync("/api/login", content);


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
