namespace TesteandoMVC.Web.Services
{
    public class TestDoublesService : ITestDoublesService
    {
        private readonly ILogger<TestDoublesService> _logger;
        private static readonly Dictionary<string, string> _datos = new();

        public TestDoublesService(ILogger<TestDoublesService> logger)
        {
            _logger = logger;
        }

        // DUMMY: logger se pasa al método pero no se usa
        public string ProcesarTexto(ILogger logger, string texto)
        {
            // El logger dummy no se usa aquí - solo se pasa como parámetro
            return texto.ToUpper();
        }

        // STUB: siempre retorna valores predefinidos
        public decimal CalcularDescuento(string tipo)
        {
            return tipo switch
            {
                "VIP" => 0.20m,      // 20%
                "REGULAR" => 0.10m,  // 10%
                _ => 0.05m           // 5%
            };
        }

        // FAKE: implementación real pero simple (en memoria)
        public void GuardarDato(string key, string value)
        {
            _datos[key] = value;
        }

        public string ObtenerDato(string key)
        {
            return _datos.TryGetValue(key, out var value) ? value : "No encontrado";
        }

        // MOCK: método que queremos verificar en tests
        public void EnviarNotificacion(string mensaje)
        {
            _logger.LogInformation($"Enviando notificación: {mensaje}");
            // En tests, mockeamos este método para verificar que se llama
        }

        // SPY: método con logs que inspeccionamos en tests
        public bool ValidarYProcesar(string input)
        {
            _logger.LogInformation("Iniciando validación");
            
            if (string.IsNullOrEmpty(input))
            {
                _logger.LogWarning("Input vacío detectado");
                return false;
            }
            
            _logger.LogInformation($"Procesando input: {input}");
            _logger.LogInformation("Procesamiento completado exitosamente");
            
            return true;
        }
    }
}
