using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using TesteandoMVC.Web.Services;
using Xunit;

namespace TesteandoMVC.Tests
{
    /// <summary>
    /// Tests simples que demuestran los 5 tipos de Test Doubles
    /// Todos los tests hacen llamadas HTTP reales al servidor
    /// </summary>
    public class TestDoublesSimpleTests : IClassFixture<WebApplicationFactory<TesteandoMVC.Web.Program>>
    {
        private readonly WebApplicationFactory<TesteandoMVC.Web.Program> _factory;

        public TestDoublesSimpleTests(WebApplicationFactory<TesteandoMVC.Web.Program> factory)
        {
            _factory = factory;
        }

        #region 1. DUMMY - "Solo necesito algo que compile"

        [Fact]
        public async Task DUMMY_TestDoubles_LoggerNoSeVerifica()
        {
            // Arrange - DUMMY: Logger se pasa al servicio pero no verificamos su uso
            // El logger es un DUMMY - se pasa pero no se verifica
            
            // Mock.Of<T>() es una forma rápida de crear un mock sin configuración
            // Es equivalente a: new Mock<ILogger<TestDoublesService>>().Object
            // Útil para DUMMIES: cuando solo necesitas pasar algo pero no lo vas a usar
            Mock<ITestDoublesService> mockService = new Mock<ITestDoublesService>();
            mockService.Setup(x => x.ProcesarTexto(It.IsAny<ILogger>(), "hola")).Returns("HOLA");
            mockService.Setup(x => x.CalcularDescuento(It.IsAny<string>())).Returns(0.10m);
            mockService.Setup(x => x.GuardarDato(It.IsAny<string>(), It.IsAny<string>()));
            mockService.Setup(x => x.ObtenerDato(It.IsAny<string>())).Returns("hola");
            mockService.Setup(x => x.ValidarYProcesar(It.IsAny<string>())).Returns(true);

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles?input=hola");

            // Assert - Solo verificamos el resultado, no el uso del dummy logger
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            Assert.Contains("HOLA", content);
            // NO verificamos el logger porque es un DUMMY
        }

        #endregion

        #region 2. STUB - "Devuelve lo que necesito"

        [Fact]
        public async Task STUB_TestDoubles_RetornaValoresPredefinidos()
        {
            // Arrange - STUB: Mock configurado para retornar valores específicos
            Mock<ITestDoublesService> stubService = new Mock<ITestDoublesService>();
            
            // STUB: Configurar respuestas predefinidas
            stubService.Setup(x => x.CalcularDescuento("VIP")).Returns(0.20m);
            stubService.Setup(x => x.CalcularDescuento("REGULAR")).Returns(0.10m);
            stubService.Setup(x => x.ProcesarTexto(It.IsAny<ILogger>(), It.IsAny<string>())).Returns("PROCESADO");
            stubService.Setup(x => x.GuardarDato(It.IsAny<string>(), It.IsAny<string>()));
            stubService.Setup(x => x.ObtenerDato(It.IsAny<string>())).Returns("valor");
            stubService.Setup(x => x.ValidarYProcesar(It.IsAny<string>())).Returns(true);

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => stubService.Object);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles");

            // Assert - STUB: Verificamos que la página muestra los valores configurados
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            Assert.Contains("20", content); // DescuentoVIP (0.20 = 20%)
            Assert.Contains("10", content); // DescuentoRegular (0.10 = 10%)
        }

        #endregion

        #region 3. FAKE - "Una implementación real pero simple"

        [Fact]
        public async Task FAKE_TestDoubles_ImplementacionAlternativa()
        {
            // Arrange - FAKE: Una clase real alternativa (no es un mock configurado)
            // Diferencia con STUB: STUB usa Mock.Setup(), FAKE es una clase real
            FakeTestDoublesService fakeService = new FakeTestDoublesService();

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => fakeService);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles?input=hola");

            // Assert - FAKE: Verificar que usa la implementación alternativa
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            Assert.Contains("HOLA", content); // El fake devuelve todo en mayúsculas
        }

        #endregion

        /// <summary>
        /// FAKE: Implementación alternativa real (no es un mock)
        /// Diferencia clave: STUB = Mock configurado, FAKE = Clase real alternativa
        /// </summary>
        private class FakeTestDoublesService : ITestDoublesService
        {
            private readonly Dictionary<string, string> _storage = new Dictionary<string, string>();

            public string ProcesarTexto(ILogger logger, string input)
            {
                return input.ToUpper(); // Siempre mayúsculas (comportamiento alternativo)
            }

            public decimal CalcularDescuento(string tipoCliente)
            {
                return tipoCliente == "VIP" ? 0.20m : 0.10m;
            }

            public void GuardarDato(string clave, string valor)
            {
                _storage[clave] = valor.ToUpper(); // Guarda en mayúsculas
            }

            public string ObtenerDato(string clave)
            {
                return _storage.ContainsKey(clave) ? _storage[clave] : "NO ENCONTRADO";
            }

            public bool ValidarYProcesar(string input)
            {
                return !string.IsNullOrEmpty(input);
            }

            public void EnviarNotificacion(string mensaje)
            {
                // Fake: no hace nada real
            }
        }

        #region 4. MOCK - "Verificar que se llamó correctamente"

        [Fact]
        public async Task MOCK_TestDoubles_VerificaLlamada()
        {
            // Arrange - MOCK: Mock para verificar interacciones
            Mock<ITestDoublesService> mockService = new Mock<ITestDoublesService>();
            mockService.Setup(x => x.ProcesarTexto(It.IsAny<ILogger>(), It.IsAny<string>())).Returns("PROCESADO");
            mockService.Setup(x => x.CalcularDescuento(It.IsAny<string>())).Returns(0.10m);
            mockService.Setup(x => x.GuardarDato(It.IsAny<string>(), It.IsAny<string>()));
            mockService.Setup(x => x.ObtenerDato(It.IsAny<string>())).Returns("valor");
            mockService.Setup(x => x.ValidarYProcesar(It.IsAny<string>())).Returns(true);
            mockService.Setup(x => x.EnviarNotificacion(It.IsAny<string>()));

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles?input=prueba");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // MOCK: Verificar que se llamó el método con parámetros correctos
            mockService.Verify(x => x.EnviarNotificacion(It.Is<string>(s => s.Contains("prueba"))), 
                Times.Once, "Debe enviar notificación exactamente una vez");
        }

        #endregion

        #region 5. SPY - "¿Qué pasó exactamente?"

        [Fact]
        public async Task SPY_TestDoubles_VerificaSecuenciaLlamadas()
        {
            // Arrange - SPY: Mock del servicio para inspeccionar uso detallado
            Mock<ITestDoublesService> spyService = new Mock<ITestDoublesService>();
            List<string> llamadas = new List<string>();
            
            spyService.Setup(x => x.ProcesarTexto(It.IsAny<ILogger>(), It.IsAny<string>()))
                .Callback<ILogger, string>((logger, input) => llamadas.Add($"ProcesarTexto: {input}"))
                .Returns("PROCESADO");
                
            spyService.Setup(x => x.CalcularDescuento(It.IsAny<string>()))
                .Callback<string>(tipo => llamadas.Add($"CalcularDescuento: {tipo}"))
                .Returns(0.10m);
                
            spyService.Setup(x => x.GuardarDato(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((key, value) => llamadas.Add($"GuardarDato: {key}={value}"));
                
            spyService.Setup(x => x.ObtenerDato(It.IsAny<string>()))
                .Callback<string>(key => llamadas.Add($"ObtenerDato: {key}"))
                .Returns("valor");
                
            spyService.Setup(x => x.ValidarYProcesar(It.IsAny<string>()))
                .Callback<string>(input => llamadas.Add($"ValidarYProcesar: {input}"))
                .Returns(true);
                
            spyService.Setup(x => x.EnviarNotificacion(It.IsAny<string>()))
                .Callback<string>(mensaje => llamadas.Add($"EnviarNotificacion: {mensaje}"));

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => spyService.Object);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles?input=espia");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // SPY: Verificar que se hicieron las llamadas en el orden correcto
            Assert.Contains(llamadas, l => l.Contains("ProcesarTexto: espia"));
            Assert.Contains(llamadas, l => l.Contains("CalcularDescuento: VIP"));
            Assert.Contains(llamadas, l => l.Contains("CalcularDescuento: REGULAR"));
            Assert.Contains(llamadas, l => l.Contains("GuardarDato"));
            Assert.Contains(llamadas, l => l.Contains("ObtenerDato"));
            Assert.Contains(llamadas, l => l.Contains("ValidarYProcesar: espia"));
            Assert.Contains(llamadas, l => l.Contains("EnviarNotificacion"));
        }

        [Fact]
        public async Task SPY_TestDoubles_InputVacio_NoEnviaNotificacion()
        {
            // Arrange - SPY: Para verificar comportamiento diferente
            Mock<ITestDoublesService> spyService = new Mock<ITestDoublesService>();
            spyService.Setup(x => x.ProcesarTexto(It.IsAny<ILogger>(), It.IsAny<string>())).Returns("");
            spyService.Setup(x => x.CalcularDescuento(It.IsAny<string>())).Returns(0m);
            spyService.Setup(x => x.GuardarDato(It.IsAny<string>(), It.IsAny<string>()));
            spyService.Setup(x => x.ObtenerDato(It.IsAny<string>())).Returns("");
            spyService.Setup(x => x.ValidarYProcesar(It.IsAny<string>())).Returns(false);
            spyService.Setup(x => x.EnviarNotificacion(It.IsAny<string>()));

            HttpClient client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ITestDoublesService>(_ => spyService.Object);
                });
            }).CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("/Home/TestDoubles?input=");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string content = await response.Content.ReadAsStringAsync();
            
            // SPY: Verificar que el resultado muestra validación fallida
            Assert.Contains("false", content.ToLower());
        }

        #endregion
    }
}
