using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.Services.ViewModel.MemberShipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G_NET_02_MVC01.Controllers
{
    [Authorize]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServices _memberShipServices;
        public MemberShipController(IMemberShipServices _memberShipServices)
        {
            this._memberShipServices = _memberShipServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var memberShip = await _memberShipServices.GetAllMembershipsAsync(ct);
            return View(memberShip);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _memberShipServices.CreateMembershipAsync(model, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(model);
            }
        }
    }
}
