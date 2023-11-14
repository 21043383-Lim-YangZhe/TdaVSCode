using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TDA_WebApplication.Models;
using TDA_WebApplication.Services;

namespace TDA_WebApplication.Controllers
{
    public class HomeController : Controller
    {

        private readonly IBeersService _service;

        public HomeController(IBeersService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        //private readonly ILogger<HomeController> _logger;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> ViewBeers()
        {
            var beers = await _service.Find();
            return View(beers);
        }
    }
}
