using G_NET_02_MVC01.BLL.Services.Class;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G_NET_02_MVC01.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var sessions = await _sessionServices.GetSessionsAsync(cancellationToken);
            return View(sessions);
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await PopulateSelectListsAsync(cancellationToken);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel sessionViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(cancellationToken);
                return View(sessionViewModel);
            }
            var result = await _sessionServices.CreatSessionAsync(sessionViewModel, cancellationToken);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to create session.";
            await PopulateSelectListsAsync(cancellationToken);
            return View(sessionViewModel);

        }

        private async Task PopulateSelectListsAsync(CancellationToken cancellationToken)
        {
            ViewBag.Categories = new SelectList(await _sessionServices.GetCategorySelectListAsync(cancellationToken), "Id", "CategoryName");
            ViewBag.Trainers = new SelectList(await _sessionServices.GetTrainerSelectListAsync(cancellationToken), "Id", "Name");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id, CancellationToken cancellationToken)
        {
            var session = await _sessionServices.GetSessionByIdAsync(Id, cancellationToken);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id, CancellationToken cancellationToken)
        {

            var Session = await _sessionServices.GetupdateSessionViewToAsync(Id, cancellationToken);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session can not be edit";
                return RedirectToAction(nameof(Index));

            }
            await PopulateSelectListsAsync(cancellationToken);
            return View(Session);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSessionViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(cancellationToken);
                return View(model);
            }
            var Result = await _sessionServices.updateSessionAsync(model.Id, model, cancellationToken);
            if (Result.IsSuccess)
            {
                TempData["successMesssage"] = "Session update successufly";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = Result.ErrorMessage;
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id,CancellationToken ct) 
        {
            var session = await _sessionServices.GetSessionByIdAsync(Id,ct);
            if (session == null)
            {
                TempData["ErrorMessage"] = "seession not found";
                return RedirectToAction(nameof(Index));

            }
            return View(session);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct)
        {
            var Result = await _sessionServices.RemoveSessionAsync(Id, ct);
            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess
                ? "Session Deleted Successfully!"
                : Result.ErrorMessage; return RedirectToAction(nameof(Index));
        }


    }
}
