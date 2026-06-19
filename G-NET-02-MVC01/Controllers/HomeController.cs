using G_NET_02_MVC01.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace G_NET_02_MVC01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
