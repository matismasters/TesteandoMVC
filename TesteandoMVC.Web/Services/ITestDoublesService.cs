namespace TesteandoMVC.Web.Services
{
    public interface ITestDoublesService
    {
        // Para DUMMY - logger se pasa pero no se usa
        string ProcesarTexto(ILogger logger, string texto);
        
        // Para STUB - siempre retorna valores específicos
        decimal CalcularDescuento(string tipo);
        
        // Para FAKE - implementación simple en memoria
        void GuardarDato(string key, string value);
        string ObtenerDato(string key);
        
        // Para MOCK - método que queremos verificar que se llama
        void EnviarNotificacion(string mensaje);
        
        // Para SPY - método con logs que queremos inspeccionar
        bool ValidarYProcesar(string input);
    }
}
