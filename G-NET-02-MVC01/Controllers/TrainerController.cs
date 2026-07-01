using G_NET_02_MVC01.BLL.Services.Class;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G_NET_02_MVC01.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerServices trainerServices;
        public TrainerController(ITrainerServices trainerServices)
        {
            this.trainerServices = trainerServices;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var Trainer = await trainerServices.GetTrainerAsync(cancellationToken);
            return View(Trainer);
        }

        public async Task<IActionResult> Details(int trainerId,CancellationToken cancellationToken)
        {
            var TrainerDetails = await trainerServices.GetTrainerDetailsAsync(trainerId, cancellationToken);
            if (!TrainerDetails.IsSuccess)
            {
                 NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(TrainerDetails.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int trainerId, CancellationToken ct = default)
        {
            var trainer = await trainerServices.GetTrainerToUpdateAsync(trainerId, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerServices.CreateTrainerAsync(model, cancellationToken);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage!);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Something Went Wrong Please Try Again";
                return View(model);
            }
            var result = await trainerServices.UpdateTrainerAsync(model.Id, model, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(model);
            }
            TempData["SuccessMessage"] = "Trainer Was Updated Successfully";
            return RedirectToAction(nameof(Index));
        }



        // Edit - Display Edit Form => GET
  
        [HttpGet]
       
        public async Task<IActionResult> Delete(int trainerId, CancellationToken ct)
        {
            var result = await trainerServices.GetTrainerDetailsAsync(trainerId, ct);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct)
        {
            var Result = await trainerServices.RemoveUpdateAsync(Id, ct);
            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess
                ? "Trainer Deleted Successfully!"
                : Result.ErrorMessage; return RedirectToAction(nameof(Index));
        }
    }
}

