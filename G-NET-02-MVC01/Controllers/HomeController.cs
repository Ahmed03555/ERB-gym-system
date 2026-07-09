using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace G_NET_02_MVC01.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsServices _analyticsServices;

        public HomeController(ILogger<HomeController> logger, IAnalyticsServices analyticsServices)
        {
            _logger = logger;
            _analyticsServices = analyticsServices;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var analytics = await _analyticsServices.GetAnalyticsAsync(cancellationToken);
            return View(analytics);
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
