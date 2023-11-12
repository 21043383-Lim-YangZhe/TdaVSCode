using Microsoft.AspNetCore.Mvc;
using TDA.Models;

namespace TDA.Controllers;


public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View("Index");
    }


}


