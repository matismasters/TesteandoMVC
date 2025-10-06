using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TesteandoMVC.Web.Models;
using TesteandoMVC.Web.Services;

namespace TesteandoMVC.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISimpleService _simpleService;

    public HomeController(ILogger<HomeController> logger, ISimpleService simpleService)
    {
        _logger = logger;
        _simpleService = simpleService;
    }

    public IActionResult Index()
    {
        ViewBag.ShowMessage = _simpleService.HoraEsPar();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult NumeroAleatorio()
    {
        ViewBag.NumeroAleatorio = _simpleService.NumeroAleatorio();
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ValidarLogin(string usuario, string password)
    {
        bool esValido = _simpleService.ValidarUsuario(usuario, password);
        
        ViewBag.EsValido = esValido;
        ViewBag.Mensaje = esValido ? 
            "¡Bienvenido! Has iniciado sesión correctamente." : 
            "Usuario o contraseña incorrectos. Intenta de nuevo.";
        
        return View("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
