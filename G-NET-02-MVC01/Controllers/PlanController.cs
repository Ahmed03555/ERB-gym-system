
using Microsoft.EntityFrameworkCore;
using G_NET_02_MVC01.Models;
using Microsoft.AspNetCore.Mvc;
using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Classes;

namespace G_NET_02_MVC01.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepositorie<Plans> planRepositorie;
        public PlanController(IGenericRepositorie<Plans> _planRepositorie)
        {
            planRepositorie = _planRepositorie;
        }
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plan = await planRepositorie.GetAll(false, token);
            return View(plan);
        }
        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepositorie.GetById(id);
            if (plan == null)
                RedirectToAction(nameof(Index));
            return View(plan);

        }
    }
}
