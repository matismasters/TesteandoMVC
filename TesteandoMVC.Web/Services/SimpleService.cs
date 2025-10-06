namespace TesteandoMVC.Web.Services
{
    public interface ISimpleService
    {
        bool HoraEsPar();
    }

    public class SimpleService : ISimpleService
    {
        public bool HoraEsPar()
        {
            // Si la hora es un numero par, devuelve true, sino devuelve false
            return DateTime.Now.Hour % 2 == 0;
        }
    }
}
