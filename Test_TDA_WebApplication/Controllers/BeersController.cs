using Microsoft.AspNetCore.Mvc;
using TDA_WebApplication.Services;

namespace TDA_WebApplication.Controllers
{
    public class BeersController : Controller
    {
        private readonly IBeersService _service;

        public BeersController(IBeersService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<IActionResult> ViewBeers()
        {
            var beers = await _service.Find();
            return View(beers);
        }
    }
    }
