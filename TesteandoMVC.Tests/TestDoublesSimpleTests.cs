using Microsoft.Extensions.Logging;
using Moq;
using TesteandoMVC.Web.Services;
using FluentAssertions;
using Xunit;

namespace TesteandoMVC.Tests
{
    /// <summary>
    /// Tests simples que demuestran los 5 tipos de Test Doubles
    /// </summary>
    public class TestDoublesSimpleTests
    {
        #region 1. DUMMY - "Solo necesito algo que compile"

        [Fact]
        public void DUMMY_ProcesarTexto_LoggerNoSeUsa()
        {
            // Arrange - DUMMY: Logger se pasa pero no se usa
            var dummyLogger = new Mock<ILogger>().Object; // DUMMY - no verificamos su uso
            var testService = new TestDoublesService(Mock.Of<ILogger<TestDoublesService>>());

            // Act
            var resultado = testService.ProcesarTexto(dummyLogger, "hola mundo");

            // Assert - Solo verificamos el resultado, no el uso del dummy logger
            resultado.Should().Be("HOLA MUNDO");
            // NO verificamos el logger porque es un DUMMY
        }

        #endregion

        #region 2. STUB - "Devuelve lo que necesito"

        [Fact]
        public void STUB_CalcularDescuento_RetornaValoresPredefinidos()
        {
            // Arrange - STUB: Mock configurado para retornar valores específicos
            var stubService = new Mock<ITestDoublesService>();
            
            // STUB: Configurar respuestas predefinidas
            stubService.Setup(x => x.CalcularDescuento("VIP")).Returns(0.20m);
            stubService.Setup(x => x.CalcularDescuento("REGULAR")).Returns(0.10m);

            // Act
            var descuentoVip = stubService.Object.CalcularDescuento("VIP");
            var descuentoRegular = stubService.Object.CalcularDescuento("REGULAR");

            // Assert - STUB: Verificamos que retorna los valores configurados
            descuentoVip.Should().Be(0.20m);
            descuentoRegular.Should().Be(0.10m);
        }

        #endregion

        #region 3. FAKE - "Una implementación real pero simple"

        [Fact]
        public void FAKE_GuardarYObtenerDato_FuncionaEnMemoria()
        {
            // Arrange - FAKE: Usar la implementación real (que funciona en memoria)
            var fakeService = new TestDoublesService(Mock.Of<ILogger<TestDoublesService>>());

            // Act - FAKE: Usar la implementación real
            fakeService.GuardarDato("clave1", "valor1");
            var resultado = fakeService.ObtenerDato("clave1");
            var noEncontrado = fakeService.ObtenerDato("no_existe");

            // Assert - FAKE: Verificar que funciona realmente
            resultado.Should().Be("valor1");
            noEncontrado.Should().Be("No encontrado");
        }

        #endregion

        #region 4. MOCK - "Verificar que se llamó correctamente"

        [Fact]
        public void MOCK_EnviarNotificacion_VerificaLlamada()
        {
            // Arrange - MOCK: Mock para verificar interacciones
            var mockService = new Mock<ITestDoublesService>(); // MOCK - verificaremos llamadas

            // Act
            mockService.Object.EnviarNotificacion("Mensaje de prueba");

            // Assert - MOCK: Verificar que se llamó el método con parámetros correctos
            mockService.Verify(x => x.EnviarNotificacion("Mensaje de prueba"), 
                Times.Once, "Debe enviar notificación exactamente una vez");
        }

        #endregion

        #region 5. SPY - "¿Qué pasó exactamente?"

        [Fact]
        public void SPY_ValidarYProcesar_VerificaSecuenciaLogs()
        {
            // Arrange - SPY: Mock del logger para inspeccionar uso detallado
            var spyLogger = new Mock<ILogger<TestDoublesService>>();
            var service = new TestDoublesService(spyLogger.Object); // SPY - inspeccionaremos su uso

            // Act
            var resultado = service.ValidarYProcesar("input válido");

            // Assert - SPY: Verificar secuencia específica de logs
            resultado.Should().BeTrue();

            // SPY: Verificar que se logearon los pasos en orden correcto
            spyLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Iniciando validación")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Debe logear el inicio");

            spyLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Procesando input: input válido")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Debe logear el procesamiento");

            spyLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Procesamiento completado exitosamente")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Debe logear el éxito");
        }

        [Fact]
        public void SPY_ValidarYProcesar_InputVacio_LogeaWarning()
        {
            // Arrange - SPY: Para verificar comportamiento diferente
            var spyLogger = new Mock<ILogger<TestDoublesService>>();
            var service = new TestDoublesService(spyLogger.Object);

            // Act
            var resultado = service.ValidarYProcesar("");

            // Assert
            resultado.Should().BeFalse();

            // SPY: Verificar que se logea el warning
            spyLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Input vacío detectado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once, "Debe logear warning para input vacío");
        }

        #endregion
    }
}
