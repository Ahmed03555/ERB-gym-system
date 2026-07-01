
using Microsoft.EntityFrameworkCore;
using G_NET_02_MVC01.Models;
using Microsoft.AspNetCore.Mvc;
using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Classes;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.PlanViewModels;

namespace G_NET_02_MVC01.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepositorie<Plans> planRepositorie;
        private readonly IPlanServices planServices;
        public PlanController(IGenericRepositorie<Plans> _planRepositorie, IPlanServices _planServices)
        {
            planRepositorie = _planRepositorie;
            planServices = _planServices;
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            var plan = await planServices.GetPlanToUpdateAsync(id, token);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                RedirectToAction(nameof(Index));
            }
                
            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> Edit( UpdatePlanViewModel model, CancellationToken token)
        {
            var result = await planServices.UpdatePlanAsync(model.Id, model, token);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            TempData["SuccessMessage"] = "Plan updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
